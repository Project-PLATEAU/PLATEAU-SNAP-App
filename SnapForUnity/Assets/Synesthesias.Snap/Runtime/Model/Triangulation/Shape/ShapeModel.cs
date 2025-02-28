using Cysharp.Threading.Tasks;
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
using System.Threading;

namespace Synesthesias.Snap.Runtime
{
    public class ShapeModel
    {
        // Triangulationを行う関数
        public async UniTask CreateShapeAsync(
            Shape shapeData,
            Mesh mesh,
            CancellationToken cancellationToken)
        {
            if (shapeData.hull.Length < 1)
            {
                Debug.LogWarning("頂点が1つもありません");
                return;
            }

            //頂点の重複がある時，クラッシュするのを防止する．
            if (await IsOverlappingVerticesAsync(shapeData, cancellationToken))
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

            await CreateMesh(shapeData, mesh, cancellationToken);
        }

        private async UniTask CreateMesh(
            Shape shapeData,
            Mesh mesh,
            CancellationToken cancellationToken)
        {
            var iGeom = IntGeom.DefGeom;
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var pShape = ToPlainShape(
                iGeom: iGeom,
                Allocator.Persistent,
                hull: shapeData.hull,
                holes: shapeData.holes);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            var extraPoints = new NativeArray<IntVector>(0, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var minX = shapeData.hull.Min(v => v.x);
            var maxX = shapeData.hull.Max(v => v.x);
            var minY = shapeData.hull.Min(v => v.y);
            var maxY = shapeData.hull.Max(v => v.y);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var width = maxX - minX;
            var height = maxY - minY;
            var maxDimension = math.max(width, height);
            var maxEdgeValue = maxDimension * 0.5F; // 2分の1の長さ
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            maxEdgeValue = math.clamp(maxEdgeValue, 0.5F, 10F);
            var maxEdge = iGeom.Int(maxEdgeValue);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var delaunay = pShape.Delaunay(
                maxEdge: maxEdge,
                extraPoints: extraPoints,
                allocator: Allocator.Persistent);

            pShape.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var totalArea = width * height;
            var maxAreaValue = totalArea * 0.5F; // 2分の1の面積
            maxAreaValue = math.clamp(maxAreaValue, 0.5F, 50F);
            var maxArea = iGeom.Int(maxAreaValue);

            delaunay.Tessellate(iGeom, maxArea);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            extraPoints.Dispose();
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var triangles = delaunay.Indices(Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var vertices = delaunay.Vertices(Allocator.Persistent, iGeom, 0);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            delaunay.Dispose();
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            // set each triangle as a separate mesh

            var subVertices = new NativeArray<float3>(3, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var subIndices = new NativeArray<int>(new[] { 0, 1, 2 }, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var colorMesh = new NativeColorMesh(triangles.Length, Allocator.Persistent);
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            for (int i = 0; i < triangles.Length; i += 3)
            {
                for (int j = 0; j < 3; j += 1)
                {
                    // 処理負荷を軽減するために1フレーム待機
                    await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                    var v = vertices[triangles[i + j]];
                    subVertices[j] = new float3(v.x, v.y, v.z);
                }

                var subMesh = new StaticPrimitiveMesh(subVertices, subIndices, Allocator.Persistent);
                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                colorMesh.AddAndDispose(subMesh, Color.black);
            }

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            subIndices.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            subVertices.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            vertices.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            triangles.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            colorMesh.FillAndDispose(mesh);

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
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
            var normalXY = new Vector3(0, 1, 0);
            var normalXZ = new Vector3(0, 0, -1);
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
            var normalXY = new Vector3(0, 1, 0);

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
        private async UniTask<bool> IsOverlappingVerticesAsync(
            Shape vertices2d,
            CancellationToken cancellationToken)
        {
            var hashset = new HashSet<Vector2>();
            var overlap = false;

            if (vertices2d.holes == null)
            {
                foreach (var vertex in vertices2d.hull)
                {
                    // 処理負荷を軽減するために1フレーム待機
                    await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                    if (hashset.Add(vertex))
                    {
                        continue;
                    }

                    overlap = true;
                    break;
                }
            }
            else
            {
                foreach (var hole in vertices2d.holes)
                {
                    foreach (var vertex in hole)
                    {
                        // 処理負荷を軽減するために1フレーム待機
                        await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                        if (hashset.Add(vertex))
                        {
                            continue;
                        }

                        overlap = true;
                        break;
                    }
                }

                foreach (var vertex in vertices2d.hull)
                {
                    // 処理負荷を軽減するために1フレーム待機
                    await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

                    if (hashset.Add(vertex))
                    {
                        continue;
                    }

                    overlap = true;
                    break;
                }
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