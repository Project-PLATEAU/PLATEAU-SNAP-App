using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel(Editor)
    /// </summary>
    public class EditorDetectionModel
    {
        private readonly TextureRepository textureRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly EditorWebCameraModel cameraModel;
        private readonly DetectionMenuModel menuModel;
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorDetectionModel(
            TextureRepository textureRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            EditorWebCameraModel cameraModel,
            DetectionMenuModel menuModel)
        {
            this.textureRepository = textureRepository;
            this.sceneModel = sceneModel;
            this.cameraModel = cameraModel;
            this.menuModel = menuModel;
            this.localizationModel = localizationModel;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WhenAll(localizationModel.InitializeAsync(
                    tableName: "DetectionStringTableCollection",
                    cancellation),
                cameraModel.StartAsync(cancellation));

            menuModel.AddElement(new DetectionMenuElementModel(
                text: "カメラデバイス切替",
                onClick: cameraModel.ToggleDevice));
        }

        /// <summary>
        /// メニューを表示
        /// </summary>
        public void ShowMenu()
        {
            menuModel.IsVisibleProperty.Value = true;
        }

        public Texture GetCameraTexture()
        {
            var result = cameraModel.GetCameraTexture();
            return result;
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

            sceneModel.Transition(SceneNameDefine.Validation);
        }
    }
}