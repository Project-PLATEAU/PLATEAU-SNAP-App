using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 検出画面のメニューの要素のModel
    /// </summary>
    public class DetectionMenuElementModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly Func<CancellationToken, UniTask> onClickAsync;

        /// <summary>
        /// テキスト
        /// </summary>
        public readonly ReactiveProperty<string> TextProperty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMenuElementModel(
            string text,
            Func<CancellationToken, UniTask> onClickAsync)
        {
            TextProperty = new ReactiveProperty<string>(text);
            this.onClickAsync = onClickAsync;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        /// <summary>
        /// クリックを通知
        /// </summary>
        public async UniTask ClickAsync(CancellationToken cancellationToken)
        {
            await onClickAsync.Invoke(cancellationToken);
        }
    }
}