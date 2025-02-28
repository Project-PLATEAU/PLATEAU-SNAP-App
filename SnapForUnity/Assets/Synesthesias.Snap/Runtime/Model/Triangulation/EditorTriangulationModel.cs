using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Threading;

namespace Synesthesias.Snap.Runtime
{
    public class EditorTriangulationModel : ITriangulationModel
    {
        private readonly ShapeModel shapeModel;
        private static readonly float maxEdge = 1;
        private static readonly float maxArea = 1;

        public EditorTriangulationModel(
            ShapeModel shapeModel
        )
        {
            this.shapeModel = shapeModel;
        }

        /// <summary>
        /// 三角形の配列を作成する(hullのみ)
        /// </summary>
        public async UniTask<int[]> GetTriangles(
            Vector3[] hullVertices,
            CancellationToken cancellationToken)
        {
            var mesh = new Mesh();
            mesh.MarkDynamic();

            // メッシュを生成する用の頂点座標を設定
            var rotationAxisY = shapeModel.GetRotationAxisY(hullVertices);
            var rotatedHullVertices = shapeModel.GetRotatedVertices(hullVertices, rotationAxisY);

            // メッシュ生成用に2次元座標に変換
            Shape shapeData = new Shape
            {
                hull = shapeModel.GetHullVertices2d(rotatedHullVertices),
                holes = null// shapeModel.GetHolesVertices2d(rotatedHolesVertices.ToList())
            };

            await shapeModel.CreateShapeAsync(shapeData, mesh, cancellationToken);

            return mesh.triangles;
        }

        /// <summary>
        /// メッシュを取得する
        /// </summary>
        public async UniTask<Mesh> CreateMeshAsync(
            Camera camera,
            Vector3[] hullVertices, 
            Vector3[][] holesVertices,
            CancellationToken cancellationToken)
        {
            // メッシュを生成する用の頂点座標を設定
            var rotationAxisY = shapeModel.GetRotationAxisY(hullVertices);
            var rotatedHullVertices = shapeModel.GetRotatedVertices(hullVertices, rotationAxisY);
            var rotatedHolesVertices = holesVertices.Select(vertices => shapeModel.GetRotatedVertices(vertices, rotationAxisY));

            // メッシュ生成用に2次元座標に変換
            Shape shapeData = new Shape
            {
                hull = shapeModel.GetHullVertices2d(rotatedHullVertices),
                holes = shapeModel.GetHolesVertices2d(rotatedHolesVertices.ToList())
            };

            var mesh = new Mesh();
            mesh.MarkDynamic();

            // メッシュを生成する(meshにデータが入る)
            await shapeModel.CreateShapeAsync(shapeData, mesh, cancellationToken);

            // verticesに渡す頂点を作成
            var invertRotaionMatrix = shapeModel.GetInvertRotationMatrix(rotationAxisY);
            var restoredVertices = shapeModel.GetRestoredVertices(mesh.vertices.ToList(), invertRotaionMatrix);

            mesh.vertices = restoredVertices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}