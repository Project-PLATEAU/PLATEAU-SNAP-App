using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel
    /// </summary>
    public class DetectionModel : IDetectionModel
    {
        /// <summary>
        /// 開始
        /// </summary>
        public UniTask StartAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 戻る
        /// </summary>
        public void Back()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 建物が検出されているか
        /// </summary>
        public UniTask<bool> IsDetectAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 撮影
        /// </summary>
        public UniTask CaptureAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 登録
        /// </summary>
        public UniTask RegisterAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}