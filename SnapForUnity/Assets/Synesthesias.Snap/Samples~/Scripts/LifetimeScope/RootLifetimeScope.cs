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
        [SerializeField] private ApiConfigurationScriptableObject apiConfiguration;
        [SerializeField] private ResidentView residentView;

        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<SceneModel>(Lifetime.Singleton);
            builder.Register<PlatformModel>(Lifetime.Singleton);
            builder.RegisterInstance(residentView);
            ConfigureLocalization(builder);
            ConfigureBoot(builder);
            ConfigureAPI(builder);
            ConfigureRepository(builder);
        }

        private void ConfigureLocalization(IContainerBuilder builder)
        {
            builder.Register<LocalizationModel>(Lifetime.Singleton);
        }

        private void ConfigureBoot(IContainerBuilder builder)
        {
#if UNITY_EDITOR
            builder.Register<MockServerModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
#else
            builder.Register<ServerModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
#endif
        }

        private void ConfigureAPI(IContainerBuilder builder)
        {
            var configuration = new Synesthesias.PLATEAU.Snap.Generated.Client.Configuration
            {
                BasePath = apiConfiguration.EndPoint,
                DefaultHeaders = { { apiConfiguration.ApiKeyType, apiConfiguration.ApiKeyValue } }
            };

            builder.RegisterInstance(configuration);

            builder.Register<MockEndPointModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            // TODO: APIModelに差し替える
            builder.Register<MockAPIModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private static void ConfigureRepository(IContainerBuilder builder)
        {
            builder.Register<TextureRepository>(Lifetime.Singleton);
            builder.Register<ValidationRepository>(Lifetime.Singleton);
        }
    }
}