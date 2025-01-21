using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// メイン画面のみが影響範囲のLifetimeScope
    /// </summary>
    public class MainLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private MainView mainView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MainModel>(Lifetime.Singleton);
            builder.RegisterInstance(mainView);
            builder.RegisterEntryPoint<MainPresenter>();
        }
    }
}