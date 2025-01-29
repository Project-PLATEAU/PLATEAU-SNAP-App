using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel
    /// </summary>
    public interface IDetectionModel : IAsyncStartable
    {
        /// <summary>
        /// 戻る
        /// </summary>
        void Back();

        /// <summary>
        /// 建物が検出されているか
        /// </summary>
        UniTask<bool> IsDetectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 撮影
        /// </summary>
        UniTask CaptureAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 登録
        /// </summary>
        UniTask RegisterAsync(CancellationToken cancellationToken);

        /// <summary>
        /// キャンセル
        /// </summary>
        void Cancel();
    }
}