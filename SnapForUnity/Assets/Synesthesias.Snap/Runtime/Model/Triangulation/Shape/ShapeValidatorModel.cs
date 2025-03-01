using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    public class ShapeValidatorModel
    {
        private readonly VectorCalculatorModel calculatorModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShapeValidatorModel(VectorCalculatorModel calculatorModel)
        {
            this.calculatorModel = calculatorModel;
        }

        /// <summary>
        /// 頂点の重複を検知する関数
        /// </summary>
        /// <returns>頂点が重複しているか</returns>
        public async UniTask<bool> IsOverlappingVerticesAsync(
            Vector3[] hull,
            Vector3[][] holes,
            CancellationToken cancellationToken)
        {
            var hashset = new HashSet<Vector2>();
            var overlap = false;

            if (holes == null)
            {
                foreach (var vertex in hull)
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
                foreach (var hole in holes)
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

                foreach (var vertex in hull)
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

        /// <summary>
        /// 頂点座標が反時計回りかどうかを判定する関数
        /// </summary>
        public bool IsCounterClockwise(Vector3[] hull)
        {
            var normal = calculatorModel.NormalVectorFrom3d(hull);
            var normalXY = new Vector3(0, 0, -1).normalized; //2つのベクトルの内積が-1の時，反時計まわり
            var isCounterClockwise = (Vector3.Dot(normal, normalXY) <= -1);
            return isCounterClockwise;
        }

        /// <summary>
        /// 頂点座標が反時計回りかどうかを判定する関数
        /// </summary>
        public bool IsCounterClockwise(Vector2[] hull)
        {
            var normal = calculatorModel.NormalVectorFrom2d(hull);
            var normalXY = new Vector3(0, 0, -1).normalized; //2つのベクトルの内積が-1の時，反時計まわり
            var isCounterClockwise = (Vector3.Dot(normal, normalXY) <= -1);
            return isCounterClockwise;
        }
    }
}