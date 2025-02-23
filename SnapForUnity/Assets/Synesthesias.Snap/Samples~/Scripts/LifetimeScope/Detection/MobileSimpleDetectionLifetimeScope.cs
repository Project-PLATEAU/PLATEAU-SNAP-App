using Google.XR.ARCoreExtensions;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.Snap.Runtime;
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
            ConfigureGeospatialDebug(builder);
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
            builder.Register<MobileSimpleDetectionModel>(Lifetime.Singleton);
            builder.Register<DetectionTouchModel>(Lifetime.Singleton);

            // TODO: 削除する
            builder.Register<MockValidationResultModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MobileSimpleDetectionPresenter>();
        }

        private void ConfigureDetectionMesh(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionMeshViewTemplate);
            builder.Register<MobileDetectionSimpleMeshModel>(Lifetime.Singleton);
            builder.Register<SimpleMeshModel>(Lifetime.Singleton);
        }

        private void ConfigureGeospatialDebug(IContainerBuilder builder)
        {
            builder.RegisterInstance(geospatialDebugView);
            builder.Register<GeospatialDebugModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GeospatialDebugPresenter>();
        }
    }
}