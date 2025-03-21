using Cysharp.Threading.Tasks;
using R3;
using Synesthesias.PLATEAU.Snap.Generated;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using Synesthesias.Snap.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 撮影判定画面のModel(Mock)
    /// </summary>
    public class MockValidationModel : IValidationModel, IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly TextureRepository textureRepository;
        private readonly ImageRepository imageRepository;
        private readonly ValidationRepository validationRepository;
        private readonly SceneModel sceneModel;
        private readonly PlatformModel platformModel;
        private readonly LocalizationModel localizationModel;
        private readonly ValidationDialogModel dialogModel;
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MockValidationModel(
            TextureRepository textureRepository,
            ImageRepository imageRepository,
            ValidationRepository validationRepository,
            SceneModel sceneModel,
            PlatformModel platformModel,
            LocalizationModel localizationModel,
            ValidationDialogModel dialogModel)
        {
            this.textureRepository = textureRepository;
            this.imageRepository = imageRepository;
            this.validationRepository = validationRepository;
            this.sceneModel = sceneModel;
            this.platformModel = platformModel;
            this.dialogModel = dialogModel;
            this.localizationModel = localizationModel;
            OnSubscribe();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSources.Add(source);
            var token = source.Token;

            await UniTask.WhenAll(
                localizationModel.InitializeAsync(
                    tableName: "DetectionStringTableCollection",
                    token));

            dialogModel.IsVisibleProperty.OnNext(true);
            var leftValidationText = localizationModel.Get("left_validation");
            var rightValidationText = localizationModel.Get("right_validation");
            var cancelButtonText = localizationModel.Get("cancel_button");
            var confirmButtonText = localizationModel.Get("confirm_button");

            var parameter = new ValidationDialogParameter(
                leftValidationText: leftValidationText,
                rightValidationText: rightValidationText,
                cancelButtonText: cancelButtonText,
                confirmButtonText: confirmButtonText);

            dialogModel.SetParameter(parameter);

            var validationTitleText = localizationModel.Get("validation_title");
            var validationDescriptionText = localizationModel.Get("validation_description");
            dialogModel.SetTitle(validationTitleText);
            dialogModel.SetDescription(validationDescriptionText);
            dialogModel.IsVisibleProperty.OnNext(true);
            ValidateAngle();
            ValidVertex();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            dialogModel.IsVisibleProperty.OnNext(false);
            textureRepository.Clear();

            foreach (var source in cancellationTokenSources)
            {
                source.Cancel();
            }

            cancellationTokenSources.Clear();
            dialogModel.Dispose();
            disposable.Dispose();
        }

        /// <summary>
        /// 戻る
        /// </summary>
        public void Back()
        {
            sceneModel.Transition(SceneNameDefine.Main);
        }

        public Texture GetCapturedTexture()
        {
            var result = textureRepository.GetTexture();
            return result;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public async UniTask RegisterAsync(CancellationToken cancellationToken)
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var token = source.Token;
            cancellationTokenSources.Add(source);

            try
            {
                dialogModel.SetTitle("画像登録");
                dialogModel.SetDescription("画像を送信中です...");

                var validationParameter = validationRepository.GetParameter();
                var capturedTexture = textureRepository.GetTexture() as Texture2D;

                await imageRepository.CreateBuildingImageAsyncAsync(
                    validationParameter: validationParameter,
                    texture: capturedTexture,
                    fileName: "test.png",
                    cancellationToken: token);

                dialogModel.SetTitle("画像登録完了");
                dialogModel.SetDescription("画像の登録が完了しました");
                await UniTask.WaitForSeconds(2, cancellationToken: token);
                dialogModel.IsVisibleProperty.OnNext(false);

                var previousSceneName = GetPreviousSceneName();
                sceneModel.Transition(previousSceneName);
            }
            catch (ApiException exception)
            {
                dialogModel.SetTitle(exception.ErrorCode.ToString());
                dialogModel.SetDescription(exception.ErrorContent.ToString());
                throw;
            }
            catch (BuildingImageException exception)
            {
                dialogModel.SetTitle(exception.Title);
                dialogModel.SetDescription(exception.Description);
                throw;
            }
            catch (Exception exception)
            {
                dialogModel.SetTitle(exception.Message);
                dialogModel.SetDescription("画像の登録に失敗しました");
                throw;
            }
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        public void Cancel()
        {
            var previousSceneName = GetPreviousSceneName();
            sceneModel.Transition(previousSceneName);
        }

        private string GetPreviousSceneName()
        {
            var result = platformModel.IsSupportedMobileDevice()
                ? SceneNameDefine.MobileDetection
                : SceneNameDefine.EditorDetection;

            return result;
        }

        private void OnSubscribe()
        {
            dialogModel.IsValidAsObservable()
                .Subscribe(OnIsValid)
                .AddTo(disposable);
        }

        private void OnIsValid(bool isValid)
        {
            var titleKey = isValid ? "valid_title" : "invalid_title";
            var titleText = localizationModel.Get(titleKey);
            dialogModel.SetTitle(titleText);

            if (isValid)
            {
                var validDescriptionText = localizationModel.Get("valid_description");
                dialogModel.SetDescription(validDescriptionText);
                return;
            }

            var invalidDescriptionText = localizationModel.Get("invalid_description");
            var builder = new StringBuilder(invalidDescriptionText);

            if (!dialogModel.IsRightValidProperty.Value)
            {
                builder.AppendLine("\n\n建物の面をすべて画角に収めて、なるべく正面から撮影してください。");
            }

            if (!dialogModel.IsLeftValidProperty.Value)
            {
                builder.AppendLine("\n\nなるべく建物の正面から撮影してください。");
            }

            var description = builder.ToString();
            dialogModel.SetDescription(description);
        }

        /// <summary>
        /// 撮影角度の検証
        /// </summary>
        private void ValidateAngle()
        {
            var angleResult = validationRepository.GetAngleResult();
            var isValidAngle = angleResult == MeshValidationAngleResultType.Valid;
            dialogModel.IsLeftValidProperty.OnNext(isValidAngle);
        }

        /// <summary>
        /// 面の欠けの検証
        /// </summary>
        private void ValidVertex()
        {
            var vertexResult = validationRepository.GetVertexResult();
            var isValidVertex = vertexResult == MeshValidationVertexResultType.Valid;
            dialogModel.IsRightValidProperty.OnNext(isValidVertex);
        }
    }
}