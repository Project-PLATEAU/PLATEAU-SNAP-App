using R3;
using System;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 検出画面のメニューの要素のModel
    /// </summary>
    public class DetectionMenuElementModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly Subject<Unit> clickSubject = new();

        /// <summary>
        /// テキスト
        /// </summary>
        public readonly ReactiveProperty<string> TextProperty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMenuElementModel(
            string text,
            Action onClick)
        {
            TextProperty = new ReactiveProperty<string>(text);

            clickSubject
                .Subscribe(_ => onClick())
                .AddTo(disposable);
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
        public void Click()
        {
            clickSubject.OnNext(Unit.Default);
        }
    }
}