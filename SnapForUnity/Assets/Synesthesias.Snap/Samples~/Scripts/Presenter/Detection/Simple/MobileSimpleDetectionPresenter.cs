using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のPresenter(簡易メッシュ版 - 携帯端末)
    /// </summary>
    public class MobileSimpleDetectionPresenter : IAsyncStartable
    {
        private readonly MobileSimpleDetectionModel model;
        private readonly MobileSimpleDetectionView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileSimpleDetectionPresenter(
            MobileSimpleDetectionModel model,
            MobileSimpleDetectionView view)
        {
            this.model = model;
            this.view = view;
            OnSubscribe();
            view.CameraButton.interactable = false;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await model.StartAsync(
                camera: view.ArCamera,
                cancellationToken);
        }

        private void OnSubscribe()
        {
            model.OnSelectedAsObservable()
                .Subscribe(OnOnSelected)
                .AddTo(view);

            view.TouchView.OnScreenInputAsObservable()
                .Subscribe(OnScreenInput)
                .AddTo(view);

            model.IsGeospatialVisibleAsObservable()
                .Subscribe(OnClickGeospatial)
                .AddTo(view);

            view.MenuButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickMenu())
                .AddTo(view);

            view.CameraButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickCameraAsync().Forget(Debug.LogException))
                .AddTo(view);
        }

        private void OnOnSelected(bool isSelected)
        {
            view.CameraButton.interactable = isSelected;
        }

        private void OnScreenInput(Vector2 position)
        {
            model.TouchScreen(view.ArCamera, position);
        }

        private void OnClickMenu()
        {
            model.ShowMenu();
        }

        private void OnClickGeospatial(bool isVisible)
        {
            view.GeospatialObject.SetActive(isVisible);
        }

        private async UniTask OnClickCameraAsync()
        {
            view.CameraButton.interactable = false;
            var cancellationToken = view.GetCancellationTokenOnDestroy();

            await model.CaptureAsync(
                camera: view.ArCamera,
                cancellationToken: cancellationToken);
        }
    }
}