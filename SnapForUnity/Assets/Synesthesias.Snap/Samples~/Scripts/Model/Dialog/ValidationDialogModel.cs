using R3;
using System;
using System.Linq;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 判定ダイアログのModel
    /// </summary>
    public class ValidationDialogModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly Subject<ValidationDialogParameter> parameterSubject = new();
        private readonly Subject<string> titleSubject = new();
        private readonly Subject<string> descriptionSubject = new();
        private readonly Subject<bool> isValidSubject = new();

        /// <summary>
        /// 表示するかどうかのプロパティ
        /// </summary>
        public readonly ReactiveProperty<bool> IsVisibleProperty = new();

        /// <summary>
        /// 左が有効かどうかのプロパティ
        /// </summary>
        public readonly ReactiveProperty<bool> IsLeftValidProperty = new();

        /// <summary>
        /// 右が有効かどうかのプロパティ
        /// </summary>
        public readonly ReactiveProperty<bool> IsRightValidProperty = new();

        /// <summary>
        /// 開くのObservable
        /// </summary>
        public Observable<ValidationDialogParameter> ParameterAsObservable()
            => parameterSubject;

        /// <summary>
        /// タイトルのObservable
        /// </summary>
        public Observable<string> TitleAsObservable()
            => titleSubject;

        /// <summary>
        /// 説明のObservable
        /// </summary>
        public Observable<string> DescriptionAsObservable()
            => descriptionSubject;

        /// <summary>
        /// 有効かどうかのObservable
        /// </summary>
        public Observable<bool> IsValidAsObservable()
            => isValidSubject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationDialogModel()
        {
            OnSubscribe();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        /// <summary>
        /// パラメータ設定
        /// </summary>
        public void SetParameter(ValidationDialogParameter parameter)
        {
            parameterSubject.OnNext(parameter);
        }

        /// <summary>
        /// タイトル設定
        /// </summary>
        public void SetTitle(string title)
        {
            titleSubject.OnNext(title);
        }

        /// <summary>
        /// 説明設定
        /// </summary>
        public void SetDescription(string description)
        {
            descriptionSubject.OnNext(description);
        }

        /// <summary>
        /// タイトルアイコンスプライト取得
        /// </summary>
        public Sprite GetTitleIconSprite(
            ValidationDialogIconSprite[] iconSprites,
            bool isValid)
        {
            var icon = isValid ? DialogIconDefine.Success1 : DialogIconDefine.Error1;

            var result = iconSprites
                .FirstOrDefault(iconSprite => iconSprite.Icon == icon);

            return result?.Sprite;
        }

        /// <summary>
        /// テキストアイコンスプライト取得
        /// </summary>
        public Sprite GetTextIconSprite(
            ValidationDialogIconSprite[] iconSprites,
            bool isValid)
        {
            var icon = isValid ? DialogIconDefine.Success2 : DialogIconDefine.Error2;

            var result = iconSprites
                .FirstOrDefault(iconSprite => iconSprite.Icon == icon);

            return result?.Sprite;
        }

        private void OnSubscribe()
        {
            IsLeftValidProperty
                .Zip(IsRightValidProperty, (left, right) => left && right)
                .Where(_ => IsVisibleProperty.Value)
                .Subscribe(isValid =>
                {
                    isValidSubject.OnNext(isValid);
                })
                .AddTo(disposable);
        }
    }
}