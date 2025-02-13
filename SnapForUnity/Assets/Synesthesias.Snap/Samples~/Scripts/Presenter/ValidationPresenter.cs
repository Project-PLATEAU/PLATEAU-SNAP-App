using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検証画面のPresenter
    /// </summary>
    public class ValidationPresenter : IAsyncStartable
    {
        private readonly IValidationModel model;
        private readonly ValidationView view;
        private readonly ValidationDialogView dialogView;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationPresenter(
            IValidationModel model,
            ValidationView view,
            ValidationDialogView dialogView)
        {
            this.model = model;
            this.view = view;
            this.dialogView = dialogView;
            OnSubscribe();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var texture = model.GetCapturedTexture();
            view.CapturedRawImage.texture = texture;
        }

        private void OnSubscribe()
        {
            dialogView.CancelButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickDialogCancelAsync().Forget(Debug.LogException))
                .AddTo(view);

            dialogView.ConfirmButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickDialogConfirmAsync().Forget(Debug.LogException))
                .AddTo(view);
        }

        private async UniTask OnClickDialogCancelAsync()
        {
            dialogView.CancelButton.interactable = false;
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