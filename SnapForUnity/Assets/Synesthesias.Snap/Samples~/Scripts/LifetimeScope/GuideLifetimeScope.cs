using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 利用ガイド画面のみが影響範囲のLifetimeScope
    /// </summary>
    public class GuideLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private GuideView view;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<GuideModel>(Lifetime.Singleton);
            builder.RegisterInstance(view);
            builder.RegisterEntryPoint<GuidePresenter>();
        }
    }
}