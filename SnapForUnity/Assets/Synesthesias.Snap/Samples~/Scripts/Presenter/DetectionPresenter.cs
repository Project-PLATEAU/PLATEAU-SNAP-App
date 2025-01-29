using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のPresenter
    /// </summary>
    public class DetectionPresenter : IAsyncStartable
    {
        private readonly IDetectionModel model;
        private readonly DetectionView view;
        private readonly ValidationDialogModel dialogModel;
        private readonly ValidationDialogView dialogView;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionPresenter(
            IDetectionModel model,
            DetectionView view,
            ValidationDialogModel dialogModel,
            ValidationDialogView dialogView)
        {
            this.model = model;
            this.view = view;
            this.dialogModel = dialogModel;
            this.dialogView = dialogView;
            OnSubscribe();
            view.CameraButton.interactable = false;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await model.StartAsync(cancellationToken);

            while (true)
            {
                if (dialogModel.IsVisibleProperty.Value)
                {
                    view.CameraButton.interactable = false;
                    await UniTask.WaitForSeconds(0.5F, cancellationToken: cancellationToken);
                    continue;
                }

                var isDetect = await model.IsDetectAsync(cancellationToken);
                view.CameraButton.interactable = isDetect;
            }
        }

        private void OnSubscribe()
        {
            view.BackButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickBack())
                .AddTo(view);

            view.CameraButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickCameraAsync().Forget())
                .AddTo(view);

            dialogView.CancelButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickDialogCancel())
                .AddTo(view);

            dialogView.ConfirmButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickDialogConfirmAsync().Forget())
                .AddTo(view);
        }

        private void OnClickBack()
        {
            model.Back();
        }

        private async UniTask OnClickCameraAsync()
        {
            view.CameraButton.interactable = false;
            dialogView.ConfirmButton.interactable = false;
            var cancellationToken = view.GetCancellationTokenOnDestroy();
            await model.CaptureAsync(cancellationToken);
        }

        private void OnClickDialogCancel()
        {
            model.Cancel();
        }

        private async UniTask OnClickDialogConfirmAsync()
        {
            var cancellationToken = view.GetCancellationTokenOnDestroy();

            try
            {
                dialogView.ConfirmButton.interactable = false;
                await model.RegisterAsync(cancellationToken);
            }
            catch
            {
                dialogView.ConfirmButton.interactable = true;
            }
        }
    }
}