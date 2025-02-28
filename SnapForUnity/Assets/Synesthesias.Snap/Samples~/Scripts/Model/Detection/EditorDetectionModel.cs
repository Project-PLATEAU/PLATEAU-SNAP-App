using Cysharp.Threading.Tasks;
using R3;
using Synesthesias.Snap.Runtime;
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
        private readonly IGeospatialMathModel geospatialMathModel;
        private readonly IEditorDetectionParameterModel parameterModel;
        private readonly DetectionMenuModel menuModel;
        private readonly DetectionTouchModel touchModel;
        private readonly EditorDetectionMeshModel detectionMeshModel;
        private readonly IMeshValidationModel meshValidationModel;
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
            IGeospatialMathModel geospatialMathModel,
            IEditorDetectionParameterModel parameterModel,
            EditorWebCameraModel cameraModel,
            DetectionMenuModel menuModel,
            DetectionTouchModel touchModel,
            EditorDetectionMeshModel detectionMeshModel,
            IMeshValidationModel meshValidationModel,
            MockValidationResultModel resultModel)
        {
            this.textureRepository = textureRepository;
            this.validationRepository = validationRepository;
            this.surfaceRepository = surfaceRepository;
            this.sceneModel = sceneModel;
            this.localizationModel = localizationModel;
            this.geospatialMathModel = geospatialMathModel;
            this.parameterModel = parameterModel;
            this.cameraModel = cameraModel;
            this.menuModel = menuModel;
            this.touchModel = touchModel;
            this.detectionMeshModel = detectionMeshModel;
            this.meshValidationModel = meshValidationModel;
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
            var fromGeospatialPose = geospatialMathModel.CreateGeospatialPose(
                latitude: parameterModel.FromLatitude,
                longitude: parameterModel.FromLongitude,
                altitude: parameterModel.FromAltitude,
                eunRotation: eulerRotation);

            // 始点から先のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialMathModel.CreateGeospatialPoseAtDistance(
                geospatialPose: fromGeospatialPose,
                distance: (float)parameterModel.MaxDistance);

            var meshValidationResult = meshValidationModel.Validate(
                meshTransform: selectedMeshView.MeshFilter.transform,
                mesh: selectedMeshView.MeshFilter.mesh);

            var validationParameter = new ValidationParameterModel(
                meshValidationResult: meshValidationResult,
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

            const string ID = "Empty Id ---";

            if (touchModel.ContainsMeshId(ID))
            {
                return;
            }

            var mesh = detectionMeshModel.CreateMeshAtTransform(
                id: ID,
                position: worldPosition,
                rotation: Quaternion.identity);

            touchModel.SetDetectedMeshView(mesh);
        }

        private async UniTask OnClickClearAsync(CancellationToken cancellationToken)
        {
            detectionMeshModel.Clear();
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
            var fromGeospatialPose = geospatialMathModel.CreateGeospatialPose(
                latitude: parameterModel.FromLatitude,
                longitude: parameterModel.FromLongitude,
                altitude: parameterModel.FromAltitude,
                eunRotation: eulerRotation);

            // 始点からMaxDistance(m)先のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialMathModel.CreateGeospatialPoseAtDistance(
                geospatialPose: fromGeospatialPose,
                distance: (float)parameterModel.MaxDistance);

            var surfaces = await surfaceRepository.GetVisibleSurfacesAsync(
                fromGeospatialPose: fromGeospatialPose,
                toGeospatialPose: toGeospatialPose,
                roll: camera.transform.rotation.eulerAngles.z,
                maxDistance: parameterModel.MaxDistance,
                fieldOfView: parameterModel.FieldOfView,
                cancellationToken: token);

            await UniTask.WhenAll(surfaces
                .Select(surface => OnSurfaceAsync(
                    camera: camera,
                    surface: surface,
                    eunRotation: Quaternion.identity,
                    cancellationToken: cancellationToken))
                .ToArray());

            // touchModel.SetDetectedMeshViews(meshes);
        }

        private async UniTask OnSurfaceAsync(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            if (touchModel.ContainsMeshId(surface.GmlId))
            {
                return;
            }

            var view = await detectionMeshModel.CreateMeshView(
                camera: camera,
                surface: surface,
                eunRotation: eunRotation,
                cancellationToken: cancellationToken);

            if (!view)
            {
                return;
            }

            touchModel.SetDetectedMeshView(view);
        }

        // private EditorDetectionMeshView CreateMesh(
        //     Camera camera,
        //     ISurfaceModel surface)
        // {
        //     const float DistanceFromCamera = 10.0F;

        //     // 画面内のランダムな位置にViewを配置する
        //     var screenPosition = GetRandomScreenPosition();
        //     screenPosition.z = DistanceFromCamera;
        //     var worldPosition = camera.ScreenToWorldPoint(screenPosition);

        //     var mesh = detectionMeshModel.CreateMeshAtTransform(
        //         surface: surface,
        //         position: worldPosition,
        //         rotation: Quaternion.identity);

        //     // TODO: Surfaceの情報の位置にメッシュを描画する
        //     return mesh;
        // }

        private static Vector3 GetRandomScreenPosition()
        {
            var x = UnityEngine.Random.Range(0, Screen.width);
            var y = UnityEngine.Random.Range(0, Screen.height);

            var result = new Vector3(x, y, 0);
            return result;
        }
    }
}