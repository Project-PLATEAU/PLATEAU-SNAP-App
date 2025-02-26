using System.Collections.Generic;
using iShape.Geometry;
using iShape.Geometry.Container;
using Unity.Collections;
using iShape.Mesh2d;
using iShape.Triangulation.Shape.Delaunay;
using System;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

namespace Synesthesias.Snap.Runtime
{
    public class ShapeModel
    {
        private static readonly float maxEdge = 1;
        private static readonly float maxArea = 1;
        // Triangulationを行う関数
        public void CreateShape(Shape shapeData, Mesh mesh)
        {
            //頂点の重複がある時，クラッシュするのを防止する．
            if (IsOverlappingVertices(shapeData))
            {
                Debug.LogWarning("頂点の重複が検出された為、処理を中断しました");
                return;
            }

            //shapeData.hullの頂点座標が反時計回りに格納されている時，クラッシュするのを防止する．
            if (IsCounterClockwise(shapeData))
            {
                Debug.LogWarning("頂点が反時計まわりに格納されています");
                return;
            }

            var iGeom = IntGeom.DefGeom;

            var pShape = ToPlainShape(
                iGeom: iGeom,
                Allocator.TempJob,
                hull: shapeData.hull,
                holes: shapeData.holes);

            var extraPoints = new NativeArray<IntVector>(0, Allocator.TempJob);
            var delaunay = pShape.Delaunay(iGeom.Int(maxEdge), extraPoints, Allocator.TempJob);
            delaunay.Tessellate(iGeom, maxArea);

            extraPoints.Dispose();

            var triangles = delaunay.Indices(Allocator.Temp);
            var vertices = delaunay.Vertices(Allocator.Temp, iGeom, 0);

            delaunay.Dispose();

            // set each triangle as a separate mesh

            var subVertices = new NativeArray<float3>(3, Allocator.Temp);
            var subIndices = new NativeArray<int>(new[] { 0, 1, 2 }, Allocator.Temp);

            var colorMesh = new NativeColorMesh(triangles.Length, Allocator.Temp);

            for (int i = 0; i < triangles.Length; i += 3)
            {
                for (int j = 0; j < 3; j += 1)
                {
                    var v = vertices[triangles[i + j]];
                    subVertices[j] = new float3(v.x, v.y, v.z);
                }

                var subMesh = new StaticPrimitiveMesh(subVertices, subIndices, Allocator.Temp);
                var color = new Color(0, 0, 0);

                colorMesh.AddAndDispose(subMesh, color);
            }

            subIndices.Dispose();
            subVertices.Dispose();

            vertices.Dispose();
            triangles.Dispose();
            colorMesh.FillAndDispose(mesh);
            pShape.Dispose();
        }

        private Vector3 RotateByMatrix(Vector3 vertex, Matrix4x4 rotationMatrix)
        {
            // 回転行列を適用
            Vector3 rotatedVertex = rotationMatrix.MultiplyPoint3x4(vertex);

            return rotatedVertex;
        }

        public Matrix4x4 GetInvertRotationMatrix(float angle)
        {
            // 回転軸と角度を指定（例: Y軸周りに45度回転）
            Vector3 rotationAxis = Vector3.up;

            // 回転行列を作成
            Quaternion rotationQuaternion = Quaternion.AngleAxis(angle, rotationAxis);
            Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);

            // 回転行列の逆行列を計算
            Matrix4x4 inverseRotationMatrix = rotationMatrix.inverse;

            return inverseRotationMatrix;
        }

        public Vector3[] GetRestoredVertices(List<Vector3> vertices, Matrix4x4 inverseMatrix)
        {
            var results = vertices
                .Select(vertex => RotateByMatrix(vertex, inverseMatrix))
                .ToArray();

            return results;
        }

        public Vector3[] RestoredOffsetZ(Vector3[] vertices, float offsetZ)
        {
            var results = vertices
                .Select(vertex => new Vector3(vertex.x, vertex.y, offsetZ))
                .ToArray();

            return results;
        }

        public Vector2[] GetHullVertices2d(IReadOnlyList<Vector3> hullVertices)
        {
            var results = hullVertices.Select(hullVertex => 
            {
                return new Vector2(hullVertex.x, hullVertex.y);
            }).ToArray();

            return results;
        }

        public Vector2[][] GetHolesVertices2d(List<IReadOnlyList<Vector3>> holesVertices)
        {
            var results = holesVertices.Select(holeVertices =>
            {
                return holeVertices.Select(vertex => new Vector2(vertex.x, vertex.y)).ToArray();
            }).ToArray();

            return results;
        }

        public float GetRotationAxisY(IReadOnlyList<Vector3> vertices)
        {
            // verticesの法線ベクトル
            var normal = NormalVectorFrom3d(vertices);
            var normalXY = new Vector3(0,1,0);
            var normalXZ = new Vector3(0,0,-1);
            // normalをXZ平面に投影
            var fromNormal = Vector3.ProjectOnPlane(normal, normalXY);
            // もう一方のベクトルをnormalXZに設定
            var toNormal = normalXZ;
            Debug.Log($"normal:{normal}");
            var rotationAngle = Vector3.SignedAngle(fromNormal, toNormal, normalXY);
            Debug.Log($"回転角: {rotationAngle}");

            return rotationAngle;
        }

        public IReadOnlyList<Vector3> GetRotatedVertices(IReadOnlyList<Vector3> vertices, float rotationAngle)
        {
            var results = vertices.Select(vertex =>
            {
                // 回転行列を作成(XZ平面)
                var rotationQuaternion = Quaternion.Euler(0, rotationAngle, 0);
                var rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);
                return RotateByMatrix(vertex, rotationMatrix);
            }).ToList();

            return results;
        }

        // メッシュの中心座標を取得
        public Vector3 GetMeshCenter(Vector3[] vertices)
        {
            Vector3 sum = Vector3.zero;

            if (vertices.Length == 0) return sum;

            foreach (Vector3 vertex in vertices)
            {
                sum += vertex;
            }

            return sum / vertices.Length;
        }

        // メッシュの中心座標を取得
        public (double latitude, double longitude, double altitude) GetMeshCenter(
            List<List<double>> coordinateList)
        {
            double latitudeSum = 0;
            double longitudeSum = 0;
            double altitudeSum = 0;

            foreach (var coordinate in coordinateList)
            {
                latitudeSum += coordinate[1];
                longitudeSum += coordinate[0];
                altitudeSum += coordinate[2];
            }

            return (latitudeSum / coordinateList.Count, longitudeSum / coordinateList.Count,
                altitudeSum / coordinateList.Count);
        }

        //　メッシュがカメラの方向を向いているか判定
        public bool IsFacingCamera(Mesh mesh, Camera camera)
        {
            var meshNormal = mesh.normals[0];
            var cameraNormal = camera.transform.forward;
            var normalXY = new Vector3(0,1,0);

            // meshNormalとcameraNormalをXZ平面に投影
            var fromNormal = Vector3.ProjectOnPlane(meshNormal, normalXY);
            var toNormal = Vector3.ProjectOnPlane(cameraNormal, normalXY);

            var angle = Vector3.Angle(fromNormal, toNormal);

            return angle >= 90 && angle <= 180;
        }

        // メッシュを反転
        public Mesh GetInvertMesh(Mesh mesh)
        {
            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            mesh.triangles = triangles;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;
            mesh.RecalculateBounds();

            return mesh;
        }

        // 検出可能面の法線ベクトルを求める関数
        private Vector3 NormalVectorFrom3d(IReadOnlyList<Vector3> vertices)
        {
            var n = vertices.Count;
            var normal = Vector3.zero;

            for (int i = 0; i < n; i++)
            {
                var current = vertices[i];
                var next = vertices[(i + 1) % n];

                normal += Vector3.Cross(current, next);
            }

            return normal.normalized;
        }

        private Vector3 NormalVectorFrom2d(Vector2[] vertices)
        {
            var n = vertices.Length;
            var normal = Vector3.zero;

            for (int i = 0; i < n; i++)
            {
                var current = vertices[i];
                var next = vertices[(i + 1) % n];

                normal += Vector3.Cross(current, next);
            }

            return normal.normalized;
        }

        //頂点の重複を検知する関数
        private bool IsOverlappingVertices(Shape vertices2d)
        {
            var hashset = new HashSet<Vector2>();
            var overlap = false;

            if (vertices2d.holes == null)
            {
                overlap = vertices2d.hull.ToList().Any(vertex => !hashset.Add(vertex));
            }
            else
            {
                overlap = vertices2d.holes.ToList().SelectMany(vertex => vertex).Any(vertex => !hashset.Add(vertex))
                          || vertices2d.hull.ToList().Any(vertex => !hashset.Add(vertex));
            }

            return overlap;
        }

        //shapeDataに格納されている頂点座標が反時計まわりになっていることを検知する関数
        private bool IsCounterClockwise(Shape shapeData)
        {
            var normal = NormalVectorFrom2d(shapeData.hull);
            var normalXY = new Vector3(0, 0, -1).normalized; //2つのベクトルの内積が-1の時，反時計まわり
            var isCounterClockwise = (Vector3.Dot(normal, normalXY) <= -1) ? true : false;
            return isCounterClockwise;
        }

        public PlainShape ToPlainShape(
            IntGeom iGeom,
            Allocator allocator,
            Vector2[] hull,
            Vector2[][] holes = null)
        {
            var iHull = iGeom.Int(hull);

            IntShape iShape;
            if (holes != null && holes.Length > 0)
            {
                var iHoles = iGeom.Int(holes);
                iShape = new IntShape(iHull, iHoles);
            }
            else
            {
                iShape = new IntShape(iHull, Array.Empty<IntVector[]>());
            }

            var pShape = new PlainShape(iShape, allocator);

            return pShape;
        }
    }
}