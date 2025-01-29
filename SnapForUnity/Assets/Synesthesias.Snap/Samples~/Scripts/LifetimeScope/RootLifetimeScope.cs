using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// シーンをまたいで共有できるLifeTimeScope
    /// </summary>
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private ResidentView residentView;

        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<SceneModel>(Lifetime.Singleton);
            builder.RegisterInstance(residentView);
            ConfigureLocalization(builder);
            ConfigureBoot(builder);
        }

        private void ConfigureLocalization(IContainerBuilder builder)
        {
            builder.Register<LocalizationModel>(Lifetime.Singleton);
        }

        private void ConfigureBoot(IContainerBuilder builder)
        {
            builder.Register<BootModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootPresenter>();
        }
    }
}