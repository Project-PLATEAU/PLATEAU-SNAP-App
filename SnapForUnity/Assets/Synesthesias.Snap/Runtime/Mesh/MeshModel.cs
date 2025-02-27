using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュのModel
    /// </summary>
    public class MeshModel
    {
        private readonly IGeospatialMathModel geospatialMathModel;
        private bool isCheckedVertices = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeshModel(IGeospatialMathModel geospatialMathModel)
        {
            this.geospatialMathModel = geospatialMathModel;
        }

        /// <summary>
        /// 三角形のインデックス
        /// </summary>
        public bool TryCreateFanTriangles(
            Vector3 cameraPosition,
            Vector3[] vertices,
            out int[] results)
        {
            var triangleList = new List<int>();

            if (vertices.Length < 4)
            {
                results = Array.Empty<int>();
                return false;
            }

            var vertexCount = vertices.Length;

            for (var vertexIndex = 1; vertexIndex < vertexCount - 1; vertexIndex++)
            {
                // 三角形の3頂点を取得
                var vertex1 = vertices[0];    // 扇形の中心
                var vertex2 = vertices[vertexIndex];    // 現在の頂点
                var vertex3 = vertices[vertexIndex + 1]; // 次の頂点

                // 三角形の法線を計算
                var normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

                // Camera から見た方向
                var cameraToTriangle = (vertex1 + vertex2 + vertex3) / 3 - cameraPosition;
                var isFacingCamera = Vector3.Dot(normal, cameraToTriangle) < 0;

                triangleList.Add(0);

                // 三角形をカメラの向きに合わせて追加
                if (isFacingCamera)
                {
                    triangleList.Add(vertexIndex);
                    triangleList.Add(vertexIndex + 1);
                }
                else
                {
                    triangleList.Add(vertexIndex + 1);
                    triangleList.Add(vertexIndex);
                }
            }

            results = triangleList.ToArray();
            return true;
        }

        /// <summary>
        /// メッシュの作成
        /// </summary>
        /// <param name="surface">検出された面</param>
        /// <param name="parent">Colliderを設置する親オブジェクト</param>
        /// <param name="eunRotation">カメラの向き</param>
        /// <returns></returns>
        public Mesh CreateMesh(
            ISurfaceModel surface,
            Transform parent,
            Quaternion eunRotation)
        {
            var coordinates = surface?.GetUniqueCoordinates();

            if (coordinates == null || coordinates.Count == 0)
            {
                return null; // 無効なデータの場合
            }

            var mesh = new Mesh();
            mesh.MarkDynamic();

            var vertexList = new List<Vector3>();
            var triangleList = new List<int>();

            // Hull(外周)の経度緯度高度データを取得
            var hull = coordinates[0];

            // Holes(穴)の経度緯度高度データを取得
            var holes = coordinates.Skip(1);

            // Hull(外周)の処理
            var hullVertices = ConvertToVector3List(
                hull,
                eunRotation);
            
            // Holes(穴)の処理
            var holesVertices = holes.Select(vertices => ConvertToVector3List(vertices,eunRotation));
            
            // メッシュを生成する用の頂点座標を設定
            var rotationAxisY = ShapeCalculator.GetRotationAxisY(hullVertices);
            var rotatedHullVertices = ShapeCalculator.GetRotatedVertices(hullVertices, rotationAxisY);
            var rotatedHolesVertices = holesVertices.Select(vertices => ShapeCalculator.GetRotatedVertices(vertices, rotationAxisY));

            // メッシュ生成用に2次元座標に変換
            Shape shapeData = new Shape
            {
                hull = ShapeCalculator.GetHullVertices2d(rotatedHullVertices),
                holes = ShapeCalculator.GetHolesVertices2d(rotatedHolesVertices.ToList())
            };

            // メッシュを生成(meshに生成したメッシュが入る)
            ShapeCalculator.GeneratePlainShape(shapeData, mesh);

            // verticesに渡す頂点を作成
            var invertRotaionMatrix = ShapeCalculator.GetInvertRotationMatrix(rotationAxisY);
            var restoredVertices = ShapeCalculator.GetRestoredVertices(mesh.vertices.ToList(), invertRotaionMatrix);

            // 頂点を渡す
            mesh.vertices = restoredVertices.ToArray();

            return mesh;
        }

        /// <summary>
        /// Meshの生成
        /// </summary>
        public Mesh CreateMesh(Vector3[] vertices, int[] triangles)
        {
            var result = new Mesh { vertices = vertices, triangles = triangles };
            result.RecalculateNormals();
            result.RecalculateBounds();
            return result;
        }

        /// <summary>
        /// メッシュの中心の緯度経度高度を取得
        /// </summary>
        public (double latitude, double longitude, double altitude) GetMeshCenter(
            List<List<double>> coordinateList)
        {
            return ShapeCalculator.GetMeshCenter(coordinateList);
        }

        /// <summary>
        /// GeospatialPose を Unity のローカル座標に変換し、Vector3 リストを作成
        /// </summary>
        private List<Vector3> ConvertToVector3List(
            List<List<double>> coordinateList,
            Quaternion eunRotation)
        {
            var vertices = new List<Vector3>();

            // 各頂点をVector3に変換する際に使用する原点を設定
            (var originLatitude, var originLogitude, var originAltitude) = ShapeCalculator.GetMeshCenter(coordinateList);
            // GeospatialPose を作成
            var geospatialPose = geospatialMathModel.CreateGeospatialPose(
                latitude: originLatitude, //緯度
                longitude: originLogitude, //経度
                altitude: originAltitude, //高度
                eunRotation: eunRotation);
            
            var originPosition = geospatialMathModel.GetVector3(geospatialPose);
            
            foreach (var coordinate in coordinateList)
            {
                // GeospatialPose を作成
                geospatialPose = geospatialMathModel.CreateGeospatialPose(
                    latitude: coordinate[1], //緯度
                    longitude: coordinate[0], //経度
                    altitude: coordinate[2], //高度
                    eunRotation: eunRotation);
                
                var position = geospatialMathModel.GetVector3(geospatialPose);
                vertices.Add(position - originPosition);
            }

            vertices = vertices.Distinct().ToList();

            return vertices;
        }

        /// <summary>
        /// Hull（三角形分割）のための簡単な Ear Clipping アルゴリズム
        /// </summary>
        private static List<int> TriangulateHull(
            List<Vector3> vertices, 
            int startIndex)
        {
            var triangles = new List<int>();

            if (vertices.Count < 3) return triangles;

            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles.Add(startIndex);
                triangles.Add(startIndex + i);
                triangles.Add(startIndex + i + 1);
            }

            return triangles;
        }
    }
}