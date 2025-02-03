using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のみが影響範囲のLifetimeScope(エディタ用)
    /// </summary>
    public class EditorDetectionLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private EditorDetectionView detectionView;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterInstance(detectionView);
            builder.Register<EditorWebCameraModel>(Lifetime.Singleton);
            builder.Register<EditorDetectionModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<EditorDetectionPresenter>();
        }
    }
}