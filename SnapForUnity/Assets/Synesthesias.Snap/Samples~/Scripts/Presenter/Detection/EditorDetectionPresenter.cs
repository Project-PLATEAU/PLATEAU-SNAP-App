using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のPresenter(エディタ)
    /// </summary>
    public class EditorDetectionPresenter : IAsyncStartable
    {
        private readonly EditorDetectionModel model;
        private readonly EditorDetectionView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorDetectionPresenter(
            EditorDetectionModel model,
            EditorDetectionView view)
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
            await model.StartAsync(view.MainCamera, cancellationToken);
            var texture = model.GetCameraTexture();
            view.CameraRawImage.texture = texture;
        }

        private void OnSubscribe()
        {
            model.OnSelectedAsObservable()
                .Subscribe(OnSelected)
                .AddTo(view);

            view.TouchView.OnScreenInputAsObservable()
                .Subscribe(OnScreenInput)
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

        private void OnSelected(bool isSelected)
        {
            view.CameraButton.interactable = isSelected;
        }

        private void OnScreenInput(Vector2 position)
        {
            model.TouchScreen(view.TouchView.TargetCamera, position);
        }

        private void OnClickMenu()
        {
            model.ShowMenu();
        }

        private async UniTask OnClickCameraAsync()
        {
            view.CameraButton.interactable = false;
            var cancellationToken = view.GetCancellationTokenOnDestroy();
            await model.CaptureAsync(view.MainCamera, cancellationToken);
        }
    }
}