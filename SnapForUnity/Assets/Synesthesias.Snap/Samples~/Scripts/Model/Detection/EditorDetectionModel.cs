using Cysharp.Threading.Tasks;
using R3;
using Synesthesias.PLATEAU.Snap.Generated.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel(Editor)
    /// </summary>
    public class EditorDetectionModel : IDisposable
    {
        private readonly ValidationRepository validationRepository;
        private readonly TextureRepository textureRepository;
        private readonly SurfaceRepository surfaceRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly EditorWebCameraModel cameraModel;
        private readonly EditorGeospatialModel geospatialModel;
        private readonly IEditorDetectionParameterModel parameterModel;
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
            ValidationRepository validationRepository,
            SurfaceRepository surfaceRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            EditorGeospatialModel geospatialModel,
            IEditorDetectionParameterModel parameterModel,
            EditorWebCameraModel cameraModel,
            DetectionMenuModel menuModel,
            DetectionTouchModel touchModel,
            EditorMeshModel meshModel,
            MockValidationResultModel resultModel)
        {
            this.textureRepository = textureRepository;
            this.validationRepository = validationRepository;
            this.surfaceRepository = surfaceRepository;
            this.sceneModel = sceneModel;
            this.localizationModel = localizationModel;
            this.geospatialModel = geospatialModel;
            this.parameterModel = parameterModel;
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
        public async UniTask StartAsync(
            Camera camera,
            CancellationToken cancellation)
        {
            await UniTask.WhenAll(localizationModel.InitializeAsync(
                    tableName: "DetectionStringTableCollection",
                    cancellation),
                cameraModel.StartAsync(cancellation),
                resultModel.StartAsync(cancellation));

            CreateMenu(camera);
        }

        private void CreateMenu(Camera camera)
        {
            menuModel.AddElement(new DetectionMenuElementModel(
                text: "カメラデバイス切替",
                onClickAsync: cameraModel.ToggleDeviceAsync));

            menuModel.AddElement(new DetectionMenuElementModel(
                text: "アンカーのクリア",
                onClickAsync: OnClickClearAsync));

            menuModel.AddElement(new DetectionMenuElementModel(
                text: "面検出APIデバッグ",
                onClickAsync: cancellationToken => OnClickSurfaceAPIAsync(camera, cancellationToken)));
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
        public async UniTask CaptureAsync(
            Camera camera,
            CancellationToken cancellationToken)
        {
            var selectedMeshView = touchModel.GetSelectedMeshView();

            if (selectedMeshView == null)
            {
                throw new InvalidOperationException("メッシュが選択されていません");
            }

            var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSources.Add(source);

            if (!cameraModel.TryCaptureTexture2D(out var capturedTexture))
            {
                throw new InvalidOperationException("撮影に失敗しました");
            }

            // TODO: ValidationRepositoryへ統合する
            textureRepository.SetTexture(capturedTexture);

            var eulerRotation = parameterModel.EunRotation;

            // デバッグ用のGeospatialPoseを始点とする
            var fromGeospatialPose = geospatialModel.CreateGeospatialPose(
                latitude: parameterModel.FromLatitude,
                longitude: parameterModel.FromLongitude,
                altitude: parameterModel.FromAltitude,
                eunRotation: eulerRotation);

            // デバッグ用のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialModel.CreateGeospatialPose(
                latitude: parameterModel.ToLatitude,
                longitude: parameterModel.ToLongitude,
                altitude: parameterModel.ToAltitude,
                eunRotation: eulerRotation);

            var validationParameter = new ValidationParameterModel(
                gmlId: selectedMeshView.Id,
                fromGeospatialPose: fromGeospatialPose,
                toGeospatialPose: toGeospatialPose,
                roll: camera.transform.rotation.eulerAngles.z,
                timestamp: DateTime.UtcNow);

            validationRepository.SetParameter(validationParameter);
            sceneModel.Transition(SceneNameDefine.Validation);
        }

        private void OnCreateAnchor(Camera camera, Vector3 screenPosition)
        {
            const float DistanceFromCamera = 10.0F;
            var modifiedScreenPosition = screenPosition;
            modifiedScreenPosition.z = DistanceFromCamera;
            var worldPosition = camera.ScreenToWorldPoint(modifiedScreenPosition);

            var mesh = meshModel.CreateMeshAtTransform(
                id: "Empty Id ---",
                position: worldPosition,
                rotation: Quaternion.identity);

            touchModel.SetDetectedMeshViews(new[] { mesh });
        }

        private async UniTask OnClickClearAsync(CancellationToken cancellationToken)
        {
            meshModel.Clear();
            await UniTask.Yield();
        }

        private async UniTask OnClickSurfaceAPIAsync(
            Camera camera,
            CancellationToken cancellationToken)
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSources.Add(source);
            var token = source.Token;

            var eulerRotation = parameterModel.EunRotation;

            // デバッグ用のGeospatialPoseを始点とする
            var fromGeospatialPose = geospatialModel.CreateGeospatialPose(
                latitude: parameterModel.FromLatitude,
                longitude: parameterModel.FromLongitude,
                altitude: parameterModel.FromAltitude,
                eunRotation: eulerRotation);

            // デバッグ用のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialModel.CreateGeospatialPose(
                latitude: parameterModel.ToLatitude,
                longitude: parameterModel.ToLongitude,
                altitude: parameterModel.ToAltitude,
                eunRotation: eulerRotation);

            var surfaces = await surfaceRepository.GetVisibleSurfacesAsync(
                fromGeospatialPose: fromGeospatialPose,
                toGeospatialPose: toGeospatialPose,
                roll: camera.transform.rotation.eulerAngles.z,
                maxDistance: parameterModel.MaxDistance,
                fieldOfView: parameterModel.FieldOfView,
                cancellationToken: token);

            Debug.Log($"取得した面の数: {surfaces.Count}");

            var meshes = surfaces
                .Select(surface => CreateMesh(camera, surface))
                .ToArray();

            touchModel.SetDetectedMeshViews(meshes);
        }

        private EditorDetectionMeshView CreateMesh(
            Camera camera,
            Surface surface)
        {
            const float DistanceFromCamera = 10.0F;

            // 画面内のランダムな位置にViewを配置する
            var screenPosition = GetRandomScreenPosition();
            screenPosition.z = DistanceFromCamera;
            var worldPosition = camera.ScreenToWorldPoint(screenPosition);

            var mesh = meshModel.CreateMeshAtTransform(
                id: surface.Gmlid,
                position: worldPosition,
                rotation: Quaternion.identity);

            // TODO: Surfaceの情報の位置にメッシュを描画する
            return mesh;
        }

        private static Vector3 GetRandomScreenPosition()
        {
            var x = UnityEngine.Random.Range(0, Screen.width);
            var y = UnityEngine.Random.Range(0, Screen.height);
            var result = new Vector3(x, y, 0);
            return result;
        }
    }
}