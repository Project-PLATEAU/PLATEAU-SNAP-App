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
    /// 建物検出画面のみが影響範囲のLifetimeScope(携帯端末)
    /// </summary>
    public class MobileDetectionLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private ARAnchorManager arAnchorManager;
        [SerializeField] private ARCameraManager arCameraManager;
        [SerializeField] private AREarthManager arEarthManager;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARStreetscapeGeometryManager arStreetScapeGeometryManager;
        [SerializeField] private GeospatialModel geospatialModel;
        [SerializeField] private MobileDetectionView detectionView;
        [SerializeField] private DetectionMenuView menuView;
        [SerializeField] private MobileDetectionMeshView detectionMeshViewTemplate;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            ConfigureAPI(builder);
            ConfigureRepository(builder);
            builder.Register<MobileARCameraModel>(Lifetime.Singleton);
            builder.Register<MobileDetectionMeshModel>(Lifetime.Singleton);
            ConfigureMenu(builder);
            ConfigureScreenTouch(builder);
            ConfigureAR(builder);
            ConfigureDetection(builder);
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

        private void ConfigureScreenTouch(IContainerBuilder builder)
        {
            builder.Register<ScreenTouchModel>(Lifetime.Singleton);
        }

        private void ConfigureAR(IContainerBuilder builder)
        {
            builder.RegisterInstance(arCameraManager);
            builder.RegisterInstance(arAnchorManager);
            builder.RegisterInstance(arEarthManager);
            builder.RegisterInstance(arRaycastManager);
            builder.RegisterInstance(arStreetScapeGeometryManager);
            builder.RegisterInstance(geospatialModel);
            builder.Register<GeospatialAsyncModel>(Lifetime.Singleton);
            builder.Register<MobileGeospatialMathModel>(Lifetime.Singleton);
        }

        private void ConfigureDetection(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionView);
            builder.RegisterInstance(detectionView.CameraRawImage);
            builder.Register<MobileDetectionModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MobileDetectionPresenter>();
            builder.RegisterInstance(detectionMeshViewTemplate);
            builder.Register<DetectionTouchModel>(Lifetime.Singleton);

            // TODO: 削除する
            builder.Register<MockValidationResultModel>(Lifetime.Singleton);
        }
    }
}