using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のみが影響範囲のLifetimeScope
    /// </summary>
    public class DetectionLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private DetectionView detectionView;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<DetectionModel>(Lifetime.Singleton);
            builder.RegisterInstance(detectionView);
            builder.RegisterEntryPoint<DetectionPresenter>();
        }
    }
}