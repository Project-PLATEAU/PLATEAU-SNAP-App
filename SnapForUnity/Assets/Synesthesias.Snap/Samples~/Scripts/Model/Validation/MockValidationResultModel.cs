using Cysharp.Threading.Tasks;
using R3;
using Synesthesias.Snap.Runtime;
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
        private readonly ReactiveProperty<MeshValidationAngleResultType> angleResultProperty;
        private readonly ReactiveProperty<MeshValidationVertexResultType> vertexResultProperty;
        private readonly ValidationRepository validationRepository;
        private readonly DetectionMenuModel menuModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MockValidationResultModel(
            ValidationRepository validationRepository,
            DetectionMenuModel menuModel)
        {
            this.validationRepository = validationRepository;
            this.menuModel = menuModel;
            var mockAngleResult = validationRepository.MockAngleResult;
            var mockVertexResult = validationRepository.MockVertexResult;
            angleResultProperty = new ReactiveProperty<MeshValidationAngleResultType>(mockAngleResult);
            vertexResultProperty = new ReactiveProperty<MeshValidationVertexResultType>(mockVertexResult);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
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
                text: "角度のモック: ---",
                onClickAsync: OnClickAngleAsync);

            angleResultProperty
                .Subscribe(resultType =>
                {
                    validationRepository.MockAngleResult = resultType;

                    var text = resultType switch
                    {
                        MeshValidationAngleResultType.None => "---",
                        MeshValidationAngleResultType.Invalid => "失敗",
                        MeshValidationAngleResultType.Valid => "成功",
                        _ => throw new NotImplementedException($"未実装: {resultType}")
                    };

                    elementModel.TextProperty.Value = $"角度のモック: {text}";
                })
                .AddTo(disposable);

            return elementModel;
        }

        private DetectionMenuElementModel CreateToggleIsValidSurfaceMenuElementModel()
        {
            var elementModel = new DetectionMenuElementModel(
                text: "面のモック: ---",
                onClickAsync: OnClickSurfaceAsync);

            vertexResultProperty
                .Subscribe(resultType =>
                {
                    validationRepository.MockVertexResult = resultType;

                    var text = resultType switch
                    {
                        MeshValidationVertexResultType.None => "---",
                        MeshValidationVertexResultType.Invalid => "失敗",
                        MeshValidationVertexResultType.Valid => "成功",
                        _ => throw new NotImplementedException($"未実装: {resultType}")
                    };

                    elementModel.TextProperty.Value = $"面のモック: {text}";
                }).AddTo(disposable);

            return elementModel;
        }

        private async UniTask OnClickAngleAsync(CancellationToken cancellationToken)
        {
            var size = Enum.GetValues(typeof(MeshValidationAngleResultType)).Length;
            var next = ((int)angleResultProperty.Value + 1) % size;
            angleResultProperty.Value = (MeshValidationAngleResultType)next;
            await UniTask.Yield();
        }

        private async UniTask OnClickSurfaceAsync(CancellationToken cancellationToken)
        {
            var size = Enum.GetValues(typeof(MeshValidationVertexResultType)).Length;
            var next = ((int)vertexResultProperty.Value + 1) % size;
            vertexResultProperty.Value = (MeshValidationVertexResultType)next;
            await UniTask.Yield();
        }
    }
}