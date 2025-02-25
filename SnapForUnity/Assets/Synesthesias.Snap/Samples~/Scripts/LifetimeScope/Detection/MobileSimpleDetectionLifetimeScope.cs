using Google.XR.ARCoreExtensions;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.Snap.Runtime;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のLifetimeScope(簡易メッシュ版 - 携帯端末)
    /// </summary>
    public class MobileSimpleDetectionLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private GeospatialMainLoopView geospatialMainLoopView;
        [SerializeField] private ARSession arSession;
        [SerializeField] private ARCoreExtensions arCoreExtensions;
        [SerializeField] private ARAnchorManager arAnchorManager;
        [SerializeField] private AREarthManager arEarthManager;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private MobileSimpleDetectionView detectionView;
        [SerializeField] private DetectionMenuView menuView;
        [SerializeField] private MobileDetectionSimpleMeshView detectionMeshViewTemplate;
        [SerializeField] private GeospatialDebugView geospatialDebugView;
        [SerializeField] private Camera validationCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            ConfigureAPI(builder);
            ConfigureRepository(builder);
            builder.Register<MobileARCameraModel>(Lifetime.Singleton);
            ConfigureMenu(builder);
            ConfigureScreenTouch(builder);
            ConfigureGeospatialMainLoop(builder);
            ConfigureGeospatial(builder);
            ConfigureAR(builder);
            ConfigureDetection(builder);
            ConfigureDetectionMesh(builder);
            ConfigureValidation(builder);
            ConfigureGeospatialDebug(builder);
        }

        protected override void OnBootstrap(IObjectResolver resolver)
        {
            base.OnBootstrap(resolver);
            var repository = resolver.Resolve<ValidationRepository>();
            var mockParameter = CreateMockParameter();
            repository.SetParameter(mockParameter);
        }

        private void ConfigureAPI(IContainerBuilder builder)
        {
            var configuration = Parent.Container.Resolve<Synesthesias.PLATEAU.Snap.Generated.Client.Configuration>();
            var api = new SurfacesApi(configuration: configuration);

            builder.RegisterInstance(api)
                .AsImplementedInterfaces();
        }

        private void ConfigureRepository(IContainerBuilder builder)
        {
            builder.Register<SurfaceRepository>(Lifetime.Singleton);

            builder.Register<MeshRepository>(Lifetime.Singleton)
                .WithParameter("detectedMaterial", detectionView.DetectedMaterial)
                .WithParameter("selectedMaterial", detectionView.SelectedMaterial);
        }

        private void ConfigureMenu(IContainerBuilder builder)
        {
            builder.RegisterInstance(menuView);
            builder.Register<DetectionMenuModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DetectionMenuPresenter>();
        }

        private static void ConfigureScreenTouch(IContainerBuilder builder)
        {
            builder.Register<ScreenTouchModel>(Lifetime.Singleton);
        }

        private void ConfigureGeospatialMainLoop(IContainerBuilder builder)
        {
            builder.RegisterInstance(geospatialMainLoopView);
            builder.Register<GeospatialMainLoopModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GeospatialMainLoopPresenter>();
        }

        private static void ConfigureGeospatial(IContainerBuilder builder)
        {
            builder.RegisterInstance(GeospatialAccuracyThresholdModel.Default);
            builder.Register<GeospatialAccuracyModel>(Lifetime.Singleton);
            builder.Register<GeospatialAnchorModel>(Lifetime.Singleton);
            builder.Register<GeospatialPoseModel>(Lifetime.Singleton);

            builder.Register<MobileGeospatialSimpleMeshModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private void ConfigureAR(IContainerBuilder builder)
        {
            builder.RegisterInstance(arSession);
            builder.RegisterInstance(arCoreExtensions);
            builder.RegisterInstance(arAnchorManager);
            builder.RegisterInstance(arEarthManager);
            builder.RegisterInstance(arRaycastManager);

            builder.Register<MobileGeospatialMathModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private void ConfigureDetection(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionView);
            builder.RegisterInstance(detectionView.CameraRawImage);

            builder.Register<DetectionSettingModel>(Lifetime.Singleton)
                .WithParameter("minimumDistance", 10)
                .WithParameter("maximumDistance", 100)
                .WithParameter("incrementDistance", 10);

            builder.Register<MobileSimpleDetectionModel>(Lifetime.Singleton);
            builder.Register<DetectionTouchModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MobileSimpleDetectionPresenter>();
        }

        private void ConfigureDetectionMesh(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionMeshViewTemplate);
            builder.Register<MobileDetectionSimpleMeshModel>(Lifetime.Singleton);
            builder.Register<SimpleMeshModel>(Lifetime.Singleton);
        }

        private void ConfigureValidation(IContainerBuilder builder)
        {
            builder.RegisterInstance(MeshValidationAngleThresholdModel.Default);

            builder.Register<MeshValidationModel>(Lifetime.Singleton)
                .WithParameter("camera", validationCamera);

            builder.Register<MockValidationResultModel>(Lifetime.Singleton);
        }

        private void ConfigureGeospatialDebug(IContainerBuilder builder)
        {
            builder.RegisterInstance(geospatialDebugView);
            builder.Register<GeospatialDebugModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GeospatialDebugPresenter>();
        }

        private ValidationParameterModel CreateMockParameter()
        {
            var mainLoopState = new GeospatialMainLoopState();

            var accuracyResult = new GeospatialAccuracyResult(
                mainLoopState: mainLoopState,
                accuracyState: GeospatialAccuracyState.HighAccuracy);

            var meshValidationResult = new MeshValidationResult(
                mainLoopState: mainLoopState,
                accuracyResult: accuracyResult,
                meshAngleResultType: MeshValidationAngleResultType.Valid,
                meshVertexResultType: MeshValidationVertexResultType.Valid);

            var mockParameter = new ValidationParameterModel(
                meshValidationResult: meshValidationResult,
                gmlId: "gmlId",
                fromLongitude: 0.0,
                fromLatitude: 0.0,
                fromAltitude: 0.0,
                toLongitude: 0.0,
                toLatitude: 0.0,
                toAltitude: 0.0,
                roll: 0,
                timestamp: DateTime.Now);

            return mockParameter;
        }
    }
}