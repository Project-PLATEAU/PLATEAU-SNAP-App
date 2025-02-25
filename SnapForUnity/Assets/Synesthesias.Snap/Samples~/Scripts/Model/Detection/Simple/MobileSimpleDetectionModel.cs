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
    /// 建物検出シーンのModel(簡易メッシュ版 - 携帯端末)
    /// </summary>
    public class MobileSimpleDetectionModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();
        private readonly ValidationRepository validationRepository;
        private readonly SurfaceRepository surfaceRepository;
        private readonly TextureRepository textureRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly MobileARCameraModel cameraModel;
        private readonly DetectionMenuModel menuModel;
        private readonly DetectionSettingModel settingModel;
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly MeshValidationModel validationModel;
        private readonly GeospatialPoseModel geospatialPoseModel;
        private readonly IGeospatialMathModel geospatialMathModel;
        private readonly MobileDetectionSimpleMeshModel detectionMeshModel;
        private readonly DetectionTouchModel touchModel;
        private readonly MockValidationResultModel mockValidationResultModel;
        private CancellationTokenSource createMeshCancellationTokenSource;

        /// <summary>
        /// Geospatial情報を表示するか
        /// </summary>
        public ReactiveProperty<bool> IsGeospatialVisibleProperty
            => settingModel.IsGeospatialVisibleProperty;

        /// <summary>
        /// オブジェクトが選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => touchModel.OnSelectedAsObservable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileSimpleDetectionModel(
            ValidationRepository validationRepository,
            SurfaceRepository surfaceRepository,
            TextureRepository textureRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            MobileARCameraModel cameraModel,
            DetectionMenuModel menuModel,
            DetectionSettingModel settingModel,
            GeospatialAccuracyModel accuracyModel,
            MeshValidationModel validationModel,
            GeospatialPoseModel geospatialPoseModel,
            IGeospatialMathModel geospatialMathModel,
            MobileDetectionSimpleMeshModel detectionMeshModel,
            DetectionTouchModel touchModel,
            MockValidationResultModel mockValidationResultModel)
        {
            this.validationRepository = validationRepository;
            this.surfaceRepository = surfaceRepository;
            this.textureRepository = textureRepository;
            this.sceneModel = sceneModel;
            this.localizationModel = localizationModel;
            this.cameraModel = cameraModel;
            this.menuModel = menuModel;
            this.settingModel = settingModel;
            this.accuracyModel = accuracyModel;
            this.validationModel = validationModel;
            this.geospatialPoseModel = geospatialPoseModel;
            this.geospatialMathModel = geospatialMathModel;
            this.detectionMeshModel = detectionMeshModel;
            this.touchModel = touchModel;
            this.mockValidationResultModel = mockValidationResultModel;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();

            foreach (var source in cancellationTokenSources)
            {
                source.Cancel();
            }

            createMeshCancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(
            Camera camera,
            CancellationToken cancellation)
        {
            await UniTask.WhenAll(
                localizationModel.InitializeAsync(
                    tableName: "DetectionStringTableCollection",
                    cancellation),
                settingModel.StartAsync(cancellation),
                mockValidationResultModel.StartAsync(cancellation));

            CreateMenu(camera);

            while (!cancellation.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(1, cancellationToken: cancellation);

                // タップでアンカーを作成するモードの場合は処理をスキップ
                if (touchModel.IsTapToCreateAnchor)
                {
                    continue;
                }

                // 手動検出の場合は処理をスキップ
                if (settingModel.IsManualDetectionProperty.Value)
                {
                    continue;
                }

                try
                {
                    await DetectedAsync(camera, cancellation);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 画面のタッチ
        /// </summary>
        public void TouchScreen(Camera camera, Vector2 screenPosition)
        {
            if (touchModel.IsTapToCreateAnchor)
            {
                createMeshCancellationTokenSource?.Cancel();
                createMeshCancellationTokenSource = new CancellationTokenSource();
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

            var accuracy = accuracyModel.GetAccuracy();

            if (!accuracy.IsSuccess)
            {
                return;
            }

            // カメラのGeospatialPoseを始点とする
            var fromGeospatialPose = geospatialPoseModel.GetCameraPose();

            if (!fromGeospatialPose.IsValid())
            {
                Debug.LogWarning("カメラの始点位置が取得できませんでした");
                return;
            }

            var distance = settingModel.DistanceProperty.Value;

            // カメラの位置から先のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialMathModel.CreateGeospatialPoseAtDistance(
                geospatialPose: fromGeospatialPose,
                distance: distance);

            if (!toGeospatialPose.IsValid())
            {
                Debug.LogWarning("カメラの終点位置が取得できませんでした");
                return;
            }

            try
            {
                // メッシュを非表示にする
                detectionMeshModel.SetMeshActive(false);

                // メッシュが撮影画像に含まれないように2フレーム待機する
                await UniTask.DelayFrame(2, cancellationToken: cancellationToken);

                if (!cameraModel.TryCaptureTexture2D(out var capturedTexture))
                {
                    throw new InvalidOperationException("撮影に失敗しました");
                }

                // TODO: ValidationRepositoryへ統合する
                textureRepository.SetTexture(capturedTexture);

                var meshValidationResult = validationModel.Validate(
                    meshTransform: selectedMeshView.MeshFilter.transform,
                    mesh: selectedMeshView.MeshFilter.mesh);

                var validationParameter = new ValidationParameterModel(
                    meshValidationResult: meshValidationResult,
                    gmlId: selectedMeshView.Id,
                    fromLongitude: fromGeospatialPose.Longitude,
                    fromLatitude: fromGeospatialPose.Latitude,
                    fromAltitude: fromGeospatialPose.Altitude,
                    toLongitude: toGeospatialPose.Longitude,
                    toLatitude: toGeospatialPose.Latitude,
                    toAltitude: toGeospatialPose.Altitude,
                    roll: camera.transform.rotation.eulerAngles.z,
                    timestamp: DateTime.UtcNow);

                validationRepository.SetParameter(validationParameter);
                sceneModel.Transition(SceneNameDefine.Validation);
            }
            catch (Exception exception)
            {
                // 何かしらの原因でエラーが発生する場合は非表示にしたメッシュを表示しなおす
                detectionMeshModel.SetMeshActive(true);
                Debug.LogWarning(exception);
            }
        }

        private void CreateMenu(Camera camera)
        {
            var surfaceAPIDebugMenuElement = CreateManualDetectionMenuElementModel(camera);
            menuModel.AddElement(surfaceAPIDebugMenuElement);

            var clearAnchorMenuElement = CreateClearAnchorMenuElementModel();
            menuModel.AddElement(clearAnchorMenuElement);
        }

        private DetectionMenuElementModel CreateClearAnchorMenuElementModel()
        {
            var result = new DetectionMenuElementModel(
                text: "メッシュのクリア",
                onClickAsync: async _ =>
                {
                    detectionMeshModel.Clear();
                    await UniTask.Yield();
                });

            return result;
        }

        private DetectionMenuElementModel CreateManualDetectionMenuElementModel(Camera camera)
        {
            var result = new DetectionMenuElementModel(
                text: "建物手動検出",
                onClickAsync: cancellationToken => DetectedAsync(camera, cancellationToken));

            return result;
        }

        /// <summary>
        /// 建物検出
        /// </summary>
        private async UniTask DetectedAsync(
            Camera camera,
            CancellationToken cancellationToken)
        {
            var accuracy = accuracyModel.GetAccuracy();

            if (!accuracy.IsSuccess)
            {
                return;
            }

            // カメラのGeospatialPoseを始点とする
            var fromGeospatialPose = geospatialPoseModel.GetCameraPose();

            if (!fromGeospatialPose.IsValid())
            {
                return;
            }

            var distance = settingModel.DistanceProperty.Value;

            // カメラの位置から先のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialMathModel.CreateGeospatialPoseAtDistance(
                geospatialPose: fromGeospatialPose,
                distance: distance);

            if (!toGeospatialPose.IsValid())
            {
                return;
            }

            // 始点から終点までの範囲とカメラ方向の範囲内の面の配列を取得
            var surfaces = await surfaceRepository.GetVisibleSurfacesAsync(
                fromGeospatialPose: fromGeospatialPose,
                toGeospatialPose: toGeospatialPose,
                camera: camera,
                maxDistance: distance,
                cancellationToken: cancellationToken);

            var eunRotation = Quaternion.AngleAxis(
                fromGeospatialPose.EunRotation.eulerAngles.y, Vector3.up);

            await UniTask.WhenAll(surfaces
                .Select(async surface =>
                {
                    await OnSurfaceAsync(
                        camera: camera,
                        surface: surface,
                        eunRotation: eunRotation,
                        cancellationToken: cancellationToken);
                }).ToArray());
        }

        private async UniTask OnSurfaceAsync(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
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
    }
}