using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
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
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();
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
            try
            {
                model.IsVisibleProperty.OnNext(false);
                await UniTask.Yield();
                model.PopulateElements();
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
            }
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            foreach (var source in cancellationTokenSources)
            {
                source.Cancel();
                source.Dispose();
            }

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
                .Subscribe(_ => OnClickElementAsync(element, elementView).Forget(Debug.LogWarning))
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

        private async UniTask OnClickElementAsync(
            DetectionMenuElementModel elementModel,
            DetectionMenuElementView elementView)
        {
            var source = new CancellationTokenSource();
            cancellationTokenSources.Add(source);

            try
            {
                elementView.Button.interactable = false;
                await elementModel.ClickAsync(source.Token);
            }
            catch (Exception exception)
            {
                source.Cancel();
                Debug.LogWarning(exception);
            }
            finally
            {
                elementView.Button.interactable = true;
                cancellationTokenSources.Remove(source);
            }
        }
    }
}