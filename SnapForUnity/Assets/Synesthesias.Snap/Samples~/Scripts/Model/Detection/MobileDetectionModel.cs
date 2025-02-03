using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Random = UnityEngine.Random;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel(携帯端末)
    /// </summary>
    public class MobileDetectionModel
    {
        private readonly TextureRepository textureRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly MobileARCameraModel cameraModel;
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileDetectionModel(
            TextureRepository textureRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            MobileARCameraModel cameraModel)
        {
            this.textureRepository = textureRepository;
            this.sceneModel = sceneModel;
            this.cameraModel = cameraModel;
            this.localizationModel = localizationModel;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await localizationModel.InitializeAsync(
                tableName: "DetectionStringTableCollection",
                cancellation);
        }

        /// <summary>
        /// 戻る
        /// </summary>
        public void Back()
        {
            sceneModel.Transition(SceneNameDefine.Main);
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
    }
}