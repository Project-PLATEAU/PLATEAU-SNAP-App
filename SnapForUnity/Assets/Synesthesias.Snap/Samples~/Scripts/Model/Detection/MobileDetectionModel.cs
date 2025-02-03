using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using Random = UnityEngine.Random;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel(携帯端末)
    /// </summary>
    public class MobileDetectionModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly TextureRepository textureRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly MobileARCameraModel cameraModel;
        private readonly DetectionMenuModel menuModel;
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();

        /// <summary>
        /// Geospatial情報を表示するか
        /// </summary>
        public readonly ReactiveProperty<bool> IsGeospatialVisibleProperty = new(true);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileDetectionModel(
            TextureRepository textureRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            MobileARCameraModel cameraModel,
            DetectionMenuModel menuModel)
        {
            this.textureRepository = textureRepository;
            this.sceneModel = sceneModel;
            this.cameraModel = cameraModel;
            this.menuModel = menuModel;
            this.localizationModel = localizationModel;
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
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await localizationModel.InitializeAsync(
                tableName: "DetectionStringTableCollection",
                cancellation);

            OnSubscribe();
        }

        /// <summary>
        /// 戻る
        /// </summary>
        public void ShowMenu()
        {
            menuModel.IsVisibleProperty.Value = true;
        }

        /// <summary>
        /// 建物が検出されているか
        /// </summary>
        public async UniTask<bool> IsDetectAsync(CancellationToken cancellationToken)
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var token = source.Token;
            cancellationTokenSources.Add(source);
            var second = Random.Range(1, 3);
            await UniTask.WaitForSeconds(second, cancellationToken: token);
            var isDetect = Random.Range(0, 100) <= 70;
            return isDetect;
        }

        /// <summary>
        /// 撮影
        /// </summary>
        public async UniTask CaptureAsync(CancellationToken cancellationToken)
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSources.Add(source);

            if (cameraModel.TryCaptureTexture2D(out var capturedTexture))
            {
                textureRepository.SetTexture(capturedTexture);
            }

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            sceneModel.Transition(SceneNameDefine.Validation);
        }

        private void OnSubscribe()
        {
            var geospatialMenuElement = new DetectionMenuElementModel(
                text: "Geospatial: ---",
                onClick: OnClickGeospatial);

            IsGeospatialVisibleProperty
                .Subscribe(isVisible =>
                {
                    var text = isVisible ? "Geospatial: 表示" : "Geospatial: 非表示";
                    geospatialMenuElement.TextProperty.Value = text;
                })
                .AddTo(disposable);

            menuModel.AddElement(geospatialMenuElement);
        }

        private void OnClickGeospatial()
        {
            var isVisible = IsGeospatialVisibleProperty.Value;
            IsGeospatialVisibleProperty.Value = !isVisible;
        }
    }
}