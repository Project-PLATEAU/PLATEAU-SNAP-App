using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    public class ValidationDialogPresenter : IAsyncStartable, IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly ValidationDialogModel model;
        private readonly ValidationDialogView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationDialogPresenter(
            ValidationDialogModel model,
            ValidationDialogView view)
        {
            this.model = model;
            this.view = view;
            OnSubscribe();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.Yield();
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
            model.ParameterAsObservable()
                .Subscribe(OnParameter)
                .AddTo(disposable);

            model.IsVisibleProperty
                .Subscribe(OnIsVisible)
                .AddTo(disposable);

            model.TitleAsObservable()
                .Subscribe(OnTitle)
                .AddTo(disposable);

            model.DescriptionAsObservable()
                .Subscribe(OnDescription)
                .AddTo(disposable);

            model.IsLeftValidProperty
                .Subscribe(OnIsLeftValid)
                .AddTo(disposable);

            model.IsRightValidProperty
                .Subscribe(OnIsRightValid)
                .AddTo(disposable);

            model.IsValidAsObservable()
                .Subscribe(OnIsValid)
                .AddTo(disposable);
        }

        private void OnParameter(ValidationDialogParameter parameter)
        {
            view.LeftText.text = parameter.LeftValidationText;
            view.RightText.text = parameter.RightValidationText;
            view.CancelButtonText.text = parameter.CancelButtonText;
            view.ConfirmButtonText.text = parameter.ConfirmButtonText;
            view.IconImage.gameObject.SetActive(false);
            view.LeftIconImage.gameObject.SetActive(false);
            view.RightIconImage.gameObject.SetActive(false);
        }

        private void OnIsVisible(bool isVisible)
        {
            view.RootObject.SetActive(isVisible);
        }

        private void OnTitle(string title)
        {
            view.TitleText.text = title;
        }

        private void OnDescription(string description)
        {
            view.DescriptionText.text = description;
        }

        private void OnIsLeftValid(bool isValid)
        {
            var sprite = model.GetTextIconSprite(view.IconSprites, isValid);
            view.LeftIconImage.sprite = sprite;
            view.LeftIconImage.gameObject.SetActive(true);
        }

        private void OnIsRightValid(bool isValid)
        {
            var sprite = model.GetTextIconSprite(view.IconSprites, isValid);
            view.RightIconImage.sprite = sprite;
            view.RightIconImage.gameObject.SetActive(true);
        }

        private void OnIsValid(bool isValid)
        {
            var sprite = model.GetTitleIconSprite(view.IconSprites, isValid);
            view.IconImage.sprite = sprite;
            view.IconImage.gameObject.SetActive(true);
            view.ConfirmButton.interactable = isValid;
        }
    }
}