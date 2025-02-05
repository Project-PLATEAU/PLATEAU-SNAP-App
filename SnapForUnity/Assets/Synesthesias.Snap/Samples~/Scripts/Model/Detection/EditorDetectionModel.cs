using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel(Editor)
    /// </summary>
    public class EditorDetectionModel : IDisposable
    {
        private readonly TextureRepository textureRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly EditorWebCameraModel cameraModel;
        private readonly DetectionMenuModel menuModel;
        private readonly DetectionTouchModel touchModel;
        private readonly EditorMeshModel meshModel;
        private readonly MockValidationResultModel resultModel;
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();

        /// <summary>
        /// オブジェクトが選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => touchModel.OnSelectedAsObservable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorDetectionModel(
            TextureRepository textureRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            EditorWebCameraModel cameraModel,
            DetectionMenuModel menuModel,
            DetectionTouchModel touchModel,
            EditorMeshModel meshModel,
            MockValidationResultModel resultModel)
        {
            this.textureRepository = textureRepository;
            this.sceneModel = sceneModel;
            this.localizationModel = localizationModel;
            this.cameraModel = cameraModel;
            this.menuModel = menuModel;
            this.touchModel = touchModel;
            this.meshModel = meshModel;
            this.resultModel = resultModel;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            foreach (var source in cancellationTokenSources)
            {
                source.Cancel();
            }
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WhenAll(localizationModel.InitializeAsync(
                    tableName: "DetectionStringTableCollection",
                    cancellation),
                cameraModel.StartAsync(cancellation),
                resultModel.StartAsync(cancellation));

            CreateMenu();
        }

        private void CreateMenu()
        {
            menuModel.AddElement(new DetectionMenuElementModel(
                text: "カメラデバイス切替",
                onClick: cameraModel.ToggleDevice));

            menuModel.AddElement(new DetectionMenuElementModel(
                text: "アンカーのクリア",
                onClick: OnClickClear));
        }

        /// <summary>
        /// 画面をタッチ
        /// </summary>
        public void TouchScreen(Camera camera, Vector2 screenPosition)
        {
            if (touchModel.IsTapToCreateAnchor)
            {
                OnCreateAnchor(camera, screenPosition);
            }
            else
            {
                touchModel.TouchScreen(camera, screenPosition);
            }
        }

        /// <summary>
        /// メニューを表示
        /// </summary>
        public void ShowMenu()
        {
            menuModel.IsVisibleProperty.Value = true;
        }

        /// <summary>
        /// カメラのTextureを取得
        /// </summary>
        public Texture GetCameraTexture()
        {
            var result = cameraModel.GetCameraTexture();
            return result;
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

        private void OnCreateAnchor(Camera camera, Vector3 screenPosition)
        {
            const float DistanceFromCamera = 10.0F;
            var modifiedScreenPosition = screenPosition;
            modifiedScreenPosition.z = DistanceFromCamera;
            var worldPosition = camera.ScreenToWorldPoint(modifiedScreenPosition);

            var mesh = meshModel.CreateMeshAtTransform(
                position: worldPosition,
                rotation: Quaternion.identity);

            touchModel.SetDetectedMeshView(mesh);
        }

        private void OnClickClear()
        {
            meshModel.Clear();
            touchModel.Clear();
        }
    }
}