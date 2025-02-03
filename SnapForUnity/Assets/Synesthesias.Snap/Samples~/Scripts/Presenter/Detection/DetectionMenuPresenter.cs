using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメニューのPresenter
    /// </summary>
    public class DetectionMenuPresenter : IAsyncStartable, IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly DetectionMenuModel model;
        private readonly DetectionMenuView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMenuPresenter(
            DetectionMenuModel model,
            DetectionMenuView view)
        {
            this.model = model;
            this.view = view;
            OnSubscribe();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            model.IsVisibleProperty.OnNext(false);
            await UniTask.Yield();
            model.PopulateElements();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        private void OnSubscribe()
        {
            model.OnElementAddedAsObservable()
                .Subscribe(OnElementAdded)
                .AddTo(disposable);

            model.IsVisibleProperty
                .Subscribe(OnIsVisible)
                .AddTo(disposable);

            view.BackgroundButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickBackground())
                .AddTo(disposable);
        }

        private void OnElementAdded(DetectionMenuElementModel element)
        {
            var elementView = Object.Instantiate(
                view.TemplateButton,
                parent: view.ContentTransform);

            element.TextProperty
                .Subscribe(text => elementView.Text.text = text)
                .AddTo(elementView);

            elementView.Button
                .OnClickAsObservable()
                .Subscribe(_ => element.Click())
                .AddTo(elementView);

            elementView.gameObject.SetActive(true);
        }

        private void OnIsVisible(bool isVisible)
        {
            view.RootObject.SetActive(isVisible);
        }

        private void OnClickBackground()
        {
            model.IsVisibleProperty.OnNext(false);
        }
    }
}