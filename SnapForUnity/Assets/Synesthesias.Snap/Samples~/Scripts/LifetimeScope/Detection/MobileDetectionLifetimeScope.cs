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
        [SerializeField] private MobileDetectionView detectionView;
        [SerializeField] private ARCameraManager arCameraManager;
        [SerializeField] private GeospatialController geospatialController;
        [SerializeField] private DetectionMenuView menuView;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterInstance(detectionView);
            builder.RegisterInstance(arCameraManager);
            builder.RegisterInstance(geospatialController);
            builder.RegisterInstance(detectionView.CameraRawImage);
            builder.Register<MobileARCameraModel>(Lifetime.Singleton);
            builder.Register<MobileDetectionModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MobileDetectionPresenter>();
            ConfigureMenu(builder);
        }

        private void ConfigureMenu(IContainerBuilder builder)
        {
            builder.RegisterInstance(menuView);
            builder.Register<DetectionMenuModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DetectionMenuPresenter>();
        }
    }
}