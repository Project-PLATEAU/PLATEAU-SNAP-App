using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
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
            await model.StartAsync(cancellationToken);
            var texture = model.GetCameraTexture();
            view.CameraRawImage.texture = texture;

            while (true)
            {
                var isDetect = await model.IsDetectAsync(cancellationToken);
                view.CameraButton.interactable = isDetect;
                await UniTask.WaitForSeconds(0.5F, cancellationToken: cancellationToken);
            }
        }

        private void OnSubscribe()
        {
            view.BackButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickBack())
                .AddTo(view);

            view.CameraDeviceToggleButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickCameraDeviceToggle())
                .AddTo(view);

            view.CameraButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickCameraAsync().Forget())
                .AddTo(view);
        }

        private void OnClickBack()
        {
            model.Back();
        }

        private void OnClickCameraDeviceToggle()
        {
            model.ToggleCameraDevice();
        }

        private async UniTask OnClickCameraAsync()
        {
            view.CameraButton.interactable = false;
            var cancellationToken = view.GetCancellationTokenOnDestroy();
            await model.CaptureAsync(cancellationToken);
        }
    }
}