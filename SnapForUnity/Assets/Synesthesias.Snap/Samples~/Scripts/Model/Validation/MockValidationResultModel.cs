using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 検証結果のモックのModel
    /// </summary>
    public class MockValidationResultModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly ReactiveProperty<bool> isAngleValidProperty;
        private readonly ReactiveProperty<bool> isSurfaceValidProperty;
        private readonly SettingRepository settingRepository;
        private readonly DetectionMenuModel menuModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MockValidationResultModel(
            SettingRepository settingRepository,
            DetectionMenuModel menuModel)
        {
            this.settingRepository = settingRepository;
            this.menuModel = menuModel;

            var isSurfaceValid = settingRepository.GetIsSurfaceValid();
            var isAngleValid = settingRepository.GetIsAngleValid();
            isAngleValidProperty = new ReactiveProperty<bool>(isAngleValid);
            isSurfaceValidProperty = new ReactiveProperty<bool>(isSurfaceValid);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
            settingRepository.SetIsAngleValid(isAngleValidProperty.Value);
            settingRepository.SetIsSurfaceValid(isSurfaceValidProperty.Value);
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            var isValidAngleMenuElementModel = CreateToggleIsValidAngleMenuElementModel();
            menuModel.AddElement(isValidAngleMenuElementModel);

            var isValidSurfaceMenuElementModel = CreateToggleIsValidSurfaceMenuElementModel();
            menuModel.AddElement(isValidSurfaceMenuElementModel);
        }

        private DetectionMenuElementModel CreateToggleIsValidAngleMenuElementModel()
        {
            var elementModel = new DetectionMenuElementModel(
                text: "角度の検証: ---",
                onClick: () => isAngleValidProperty.Value = !isAngleValidProperty.Value);

            isAngleValidProperty
                .Subscribe(isValid =>
                {
                    var resultText = isValid ? "成功" : "失敗";
                    var text = $"角度の結果: {resultText}";
                    elementModel.TextProperty.Value = text;
                })
                .AddTo(disposable);

            return elementModel;
        }

        private DetectionMenuElementModel CreateToggleIsValidSurfaceMenuElementModel()
        {
            var elementModel = new DetectionMenuElementModel(
                text: "面の検証: ---",
                onClick: () => isSurfaceValidProperty.Value = !isSurfaceValidProperty.Value);

            isSurfaceValidProperty
                .Subscribe(isValid =>
                {
                    var resultText = isValid ? "成功" : "失敗";
                    var text = $"面の結果: {resultText}";
                    elementModel.TextProperty.Value = text;
                }).AddTo(disposable);

            return elementModel;
        }
    }
}