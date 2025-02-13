using Cysharp.Threading.Tasks;
using Google.XR.ARCoreExtensions;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel(携帯端末)
    /// </summary>
    public class MobileDetectionModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly List<CancellationTokenSource> cancellationTokenSources = new();
        private readonly SurfaceRepository surfaceRepository;
        private readonly TextureRepository textureRepository;
        private readonly SceneModel sceneModel;
        private readonly LocalizationModel localizationModel;
        private readonly MobileARCameraModel cameraModel;
        private readonly DetectionMenuModel menuModel;
        private readonly GeospatialAsyncModel geospatialAsyncModel;
        private readonly GeospatialMathModel geospatialMathModel;
        private readonly MobileMeshModel meshModel;
        private readonly DetectionTouchModel touchModel;
        private readonly MockValidationResultModel resultModel;
        private CancellationTokenSource createMeshCancellationTokenSource;

        /// <summary>
        /// Geospatial情報を表示するか
        /// </summary>
        public readonly ReactiveProperty<bool> IsGeospatialVisibleProperty = new(true);

        /// <summary>
        /// オブジェクトが選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => touchModel.OnSelectedAsObservable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileDetectionModel(
            SurfaceRepository surfaceRepository,
            TextureRepository textureRepository,
            SceneModel sceneModel,
            LocalizationModel localizationModel,
            MobileARCameraModel cameraModel,
            DetectionMenuModel menuModel,
            GeospatialAsyncModel geospatialAsyncModel,
            GeospatialMathModel geospatialMathModel,
            MobileMeshModel meshModel,
            DetectionTouchModel touchModel,
            MockValidationResultModel resultModel)
        {
            this.surfaceRepository = surfaceRepository;
            this.textureRepository = textureRepository;
            this.sceneModel = sceneModel;
            this.localizationModel = localizationModel;
            this.cameraModel = cameraModel;
            this.menuModel = menuModel;
            this.geospatialAsyncModel = geospatialAsyncModel;
            this.geospatialMathModel = geospatialMathModel;
            this.meshModel = meshModel;
            this.touchModel = touchModel;
            this.resultModel = resultModel;
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
            await UniTask.WhenAll(localizationModel.InitializeAsync(
                    tableName: "DetectionStringTableCollection",
                    cancellation),
                resultModel.StartAsync(cancellation));

            CreateMenu();

            while (!cancellation.IsCancellationRequested)
            {
                await DetectedAsync(camera, cancellation);
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
                OnCreateAnchor(screenPosition, createMeshCancellationTokenSource.Token).Forget(Debug.LogException);
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

        private void CreateMenu()
        {
            var geospatialVisibilityElementModel = CreateGeospatialVisibilityElementModel();
            menuModel.AddElement(geospatialVisibilityElementModel);

            var clearAnchorMenuElement = CreateClearAnchorMenuElementModel();
            menuModel.AddElement(clearAnchorMenuElement);
        }

        private DetectionMenuElementModel CreateGeospatialVisibilityElementModel()
        {
            var elementModel = new DetectionMenuElementModel(
                text: "Geospatial: ---",
                onClick: OnClickGeospatial);

            IsGeospatialVisibleProperty
                .Subscribe(isVisible =>
                {
                    var text = isVisible ? "Geospatial: 表示" : "Geospatial: 非表示";
                    elementModel.TextProperty.Value = text;
                })
                .AddTo(disposable);

            return elementModel;
        }

        private DetectionMenuElementModel CreateClearAnchorMenuElementModel()
        {
            var result = new DetectionMenuElementModel(
                text: "アンカーのクリア",
                onClick: meshModel.Clear);

            return result;
        }

        private void OnClickGeospatial()
        {
            var isVisible = IsGeospatialVisibleProperty.Value;
            IsGeospatialVisibleProperty.Value = !isVisible;
        }

        /// <summary>
        /// 建物検出
        /// </summary>
        private async UniTask DetectedAsync(
            Camera camera,
            CancellationToken cancellationToken)
        {
            // タップでアンカーを作成するモードの場合は処理をスキップ
            if (touchModel.IsTapToCreateAnchor)
            {
                return;
            }

            // カメラのGeospatialPoseを始点とする
            var fromGeospatialPose = geospatialAsyncModel.GetCameraGeospatialPose();

            // カメラの位置から20m先のGeospatialPoseを終点とする
            var toGeospatialPose = geospatialMathModel.CreateGeospatialPoseAtDistance(
                geospatialPose: fromGeospatialPose,
                distance: 20); // 必要に応じて距離を変更

            // 始点から終点までの範囲とカメラ方向の範囲内の面の配列を取得
            var surfaces = await surfaceRepository.GetVisibleSurfacesAsync(
                fromGeospatialPose: fromGeospatialPose,
                toGeospatialPose: toGeospatialPose,
                camera: camera,
                cancellationToken: cancellationToken);

            // CreateAnchor関連の関数でアンカーを作成する(以下の関数以外にも複数定義されている)
            var meshView = await CreateMeshViewAsync(
                geospatialPose: fromGeospatialPose,
                cancellationToken: cancellationToken);

            touchModel.SetDetectedMeshView(meshView);

            // TODO: 取得したSurfacesをMeshViewのメッシュへ描画させる
        }

        /// <summary>
        /// メッシュのViewを生成
        /// </summary>
        private async UniTask<MobileDetectionMeshView> CreateMeshViewAsync(
            GeospatialPose geospatialPose,
            CancellationToken cancellationToken)
        {
            var view = await CreateMeshViewAsync(
                latitude: geospatialPose.Latitude,
                longitude: geospatialPose.Longitude,
                altitudeAboveTerrain: geospatialPose.Altitude,
                eunRotation: geospatialPose.EunRotation,
                cancellationToken: cancellationToken);

            return view;
        }

        /// <summary>
        /// メッシュのViewを生成
        /// </summary>
        private async UniTask<MobileDetectionMeshView> CreateMeshViewAsync(
            double latitude,
            double longitude,
            double altitudeAboveTerrain,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var arGeoAnchor = await geospatialAsyncModel.CreateARGeospatialAnchorAsTerrainAsync(
                latitude: latitude,
                longitude: longitude,
                altitudeAboveTerrain: altitudeAboveTerrain,
                eunRotation: eunRotation,
                cancellationToken);

            var view = meshModel.CreateMeshAtARGeospatialAnchor(arGeoAnchor);
            return view;
        }

        private async UniTask OnCreateAnchor(
            Vector3 screenPosition,
            CancellationToken cancellationToken)
        {
            var raycastHits = new List<XRRaycastHit>();

            if (!geospatialAsyncModel.ARRaycast(screenPosition, ref raycastHits))
            {
                return;
            }

            var raycastHit = raycastHits[0];

            var arAnchor = await geospatialAsyncModel.CreateARAnchorAsStreetScapeAsync(
                raycastHit: raycastHit,
                cancellationToken: cancellationToken);

            var mesh = meshModel.CreateMeshAtARAnchor(arAnchor);

            // MEMO: 仮で小さいので大きくする
            mesh.transform.localScale *= 3F;

            touchModel.SetDetectedMeshView(mesh);
        }
    }
}