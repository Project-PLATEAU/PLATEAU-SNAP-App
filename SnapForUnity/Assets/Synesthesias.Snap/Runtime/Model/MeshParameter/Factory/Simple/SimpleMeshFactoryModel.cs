using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Threading;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// シンプルなメッシュ生成用のModel
    /// </summary>
    public class SimpleMeshFactoryModel : IMeshFactoryModel
    {
        private readonly VectorCalculatorModel calculatorModel;
        private readonly ShapeValidatorModel validatorModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleMeshFactoryModel(
            VectorCalculatorModel calculatorModel,
            ShapeValidatorModel validatorModel)
        {
            this.calculatorModel = calculatorModel;
            this.validatorModel = validatorModel;
        }

        /// <summary>
        /// メッシュの初期化
        /// </summary>
        public async UniTask<Mesh> CreateAsync(
            Vector3[] hull,
            Vector3[][] holes,
            CancellationToken cancellationToken)
        {
            if (hull.Length < 1)
            {
                Debug.LogWarning("頂点が1つもありません");
                return null;
            }

            //頂点の重複がある時，クラッシュするのを防止する．
            if (await validatorModel.IsOverlappingVerticesAsync(
                    hull: hull,
                    holes: holes,
                    cancellationToken: cancellationToken))
            {
                Debug.LogWarning("頂点の重複が検出された為、処理を中断しました");
                return null;
            }

            //shapeData.hullの頂点座標が反時計回りに格納されている時，クラッシュするのを防止する．
            if (validatorModel.IsCounterClockwise(hull))
            {
                Debug.LogWarning("頂点が反時計まわりに格納されています");
                return null;
            }

            var minX = hull.Min(v => v.x);
            var maxX = hull.Max(v => v.x);
            var minY = hull.Min(v => v.y);
            var maxY = hull.Max(v => v.y);

            // 2x2分割のための中間点を計算
            var midX = (minX + maxX) * 0.5F;
            var midY = (minY + maxY) * 0.5F;

            // 9つの頂点(3x3グリッド)
            var vertices = new Vector3[]
            {
                new(minX, minY, 0), new(midX, minY, 0), new(maxX, minY, 0), new(minX, midY, 0), new(midX, midY, 0),
                new(maxX, midY, 0), new(minX, maxY, 0), new(midX, maxY, 0), new(maxX, maxY, 0)
            };

            var triangles = new[]
            {
                0, 1, 4, 0, 4, 3, // 左下
                1, 2, 5, 1, 5, 4, // 右下
                3, 4, 7, 3, 7, 6, // 左上
                4, 5, 8, 4, 8, 7 // 右上
            };

            var rotationAxisY = calculatorModel.GetRotationAxisY(hull);

            // verticesに渡す頂点を作成
            var invertRotationMatrix = calculatorModel.GetInvertRotationMatrix(rotationAxisY);

            var restoredVertices = calculatorModel.GetRestoredVertices(
                vertices,
                invertRotationMatrix);

            var mesh = new Mesh();
            mesh.MarkDynamic();
            mesh.vertices = restoredVertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}