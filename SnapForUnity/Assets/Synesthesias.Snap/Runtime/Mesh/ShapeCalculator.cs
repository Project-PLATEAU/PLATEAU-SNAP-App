using System.Collections.Generic;
using iShape.Geometry;
using Unity.Collections;
using iShape.Mesh2d;
using iShape.Triangulation.Shape.Delaunay;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

namespace Synesthesias.Snap.Runtime
{
    public static class ShapeCalculator
    {
        private static readonly float maxEdge = 1;
        private static readonly float maxArea = 1;

        // Triangulationを行う関数
        public static void GeneratePlainShape(Shape shapeData, Mesh mesh)
        {
            //頂点の重複がある時，クラッシュするのを防止する．
            if (CheckOverlappingVertices(shapeData))
            {
                Debug.Log("頂点の重複が検出された為、処理を中断しました");
                return;
            }

            //shapeData.hullの頂点座標が反時計回りに格納されている時，クラッシュするのを防止する．
            if (CheckCounterClockwise(shapeData))
            {
                Debug.Log("頂点が反時計まわりに格納されています");
                return;
            }

            var iGeom = IntGeom.DefGeom;

            var pShape = shapeData.ToPlainShape(iGeom, Allocator.TempJob);

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

        private static Vector3 RotateByMatrix(Vector3 vertex, Matrix4x4 rotationMatrix)
        {
            // 回転行列を適用
            Vector3 rotatedVertex = rotationMatrix.MultiplyPoint3x4(vertex);

            return rotatedVertex;
        }

        public static Matrix4x4 GetInvertRotationMatrix(float angle)
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

        public static List<Vector3> GetRestoredVertices(List<Vector3> vertices, Matrix4x4 inverseMatrix)
        {
            List<Vector3> restoredVertices = new List<Vector3>();

            foreach (Vector3 vertex in vertices)
            {
                restoredVertices.Add(RotateByMatrix(vertex, inverseMatrix));
            }

            return restoredVertices;
        }

        public static Vector3[] RestoredOffsetZ(Vector3[] vertices, float offsetZ)
        {
            var restoredVertices = new List<Vector3>();

            foreach (var vertex in vertices)
            {
                restoredVertices.Add(new Vector3(vertex.x, vertex.y, offsetZ));
            }

            return restoredVertices.ToArray();
        }

        public static Vector2[] GetHullVertices2d(List<Vector3> vertices)
        {
            List<Vector2> hullVertices2d = new();

            foreach (var vertex in vertices)
            {
                Vector2 hullVertex2d = new(vertex.x, vertex.y);
                hullVertices2d.Add(hullVertex2d);
            }

            return hullVertices2d.ToArray();
        }

        public static Vector2[][] GetHolesVertices2d(List<List<Vector3>> holesVertices)
        {
            List<Vector2[]> holesVertices2d = new();

            foreach (var holeVertices in holesVertices)
            {
                var vertices = new List<Vector2>();
                foreach (var vertex in holeVertices)
                {
                    vertices.Add(new(vertex.x, vertex.y));
                }
                holesVertices2d.Add(vertices.ToArray());
            }

            return holesVertices2d.ToArray();
        }

        public static float GetRotationAxisY(List<Vector3> vertices)
        {
            Vector3 normal = NormalVectorFrom3d(vertices.ToArray());
            Debug.Log($"検出された面の法線ベクトル: {normal}");
            Vector3 normalXY = new Vector3(0, 0, -1).normalized;
            float dotProduct = Vector3.Dot(normal, normalXY);
            Debug.Log($"(0,0,-1)と{normal}の内積:{dotProduct}");
            float rotationAngle;

            if (normal.z >= 0)
            {
                rotationAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
            }
            else
            {
                rotationAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
                rotationAngle = -rotationAngle;
            }

            Debug.Log($"回転角: {rotationAngle}");

            return rotationAngle;
        }

        public static List<Vector3> GetRotatedVertices(List<Vector3> vertices, float rotationAngle)
        {
            List<Vector3> convertedVertices = new List<Vector3>();

            foreach (Vector3 vertex in vertices)
            {
                // 回転行列を作成(XZ平面)
                Quaternion rotationQuaternion = Quaternion.Euler(0, rotationAngle, 0);
                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);
                convertedVertices.Add(RotateByMatrix(vertex, rotationMatrix));
            }

            return convertedVertices;
        }

        // メッシュの中心座標を取得
        public static Vector3 GetMeshCenter(Vector3[] vertices)
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
        public static (double latitude, double longitude, double altitude) GetMeshCenter(List<List<List<double>>> coordinates)
        {
            double latitudeSum = 0;
            double longitudeSum = 0;
            double altitudeSum = 0;

            foreach (var coordinate in coordinates[0])
            {
                latitudeSum += coordinate[1];
                longitudeSum += coordinate[0];
                altitudeSum += coordinate[2];
            }

            Debug.Log(coordinates[0].Count);
            return (latitudeSum / coordinates[0].Count, longitudeSum / coordinates[0].Count, altitudeSum / coordinates[0].Count);
        }

        // 検出可能面の法線ベクトルを求める関数
        private static Vector3 NormalVectorFrom3d(Vector3[] vertices)
        {
            int n = vertices.Length;
            Vector3 normal = Vector3.zero;

            for (int i = 0; i < n; i++)
            {
                Vector3 current = vertices[i];
                Vector3 next = vertices[(i + 1) % n];

                normal += Vector3.Cross(current, next);
            }

            return normal.normalized;
        }

        private static Vector3 NormalVectorFrom2d(Vector2[] vertices)
        {
            int n = vertices.Length;
            Vector3 normal = Vector3.zero;

            for (int i = 0; i < n; i++)
            {
                Vector3 current = vertices[i];
                Vector3 next = vertices[(i + 1) % n];

                normal += Vector3.Cross(current, next);
            }

            return normal.normalized;
        }

        //頂点の重複を検知する関数
        private static bool CheckOverlappingVertices(Shape vertices2d)
        {
            var hashset = new HashSet<Vector2>();
            bool overlap = false;

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
        private static bool CheckCounterClockwise(Shape shapeData)
        {
            Vector3 normal = NormalVectorFrom2d(shapeData.hull);
            Vector3 normalXY = new Vector3(0, 0, -1).normalized; //2つのベクトルの内積が-1の時，反時計まわり
            return (Vector3.Dot(normal, normalXY) <= -1) ? true : false;
        }
    }
}