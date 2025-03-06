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
        [SerializeField] private EnvironmentScriptableObject environmentScriptableObject;
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
            ConfigureEnvironment(builder);
            ConfigureLocalization(builder);
            ConfigureAPI(builder);
            ConfigureRepository(builder);
        }

        private void ConfigureEnvironment(IContainerBuilder builder)
        {
            builder.RegisterInstance(environmentScriptableObject)
                .AsImplementedInterfaces();
        }

        private void ConfigureLocalization(IContainerBuilder builder)
        {
            builder.Register<LocalizationModel>(Lifetime.Singleton);
        }

        private void ConfigureAPI(IContainerBuilder builder)
        {
            var apiConfiguration = environmentScriptableObject.ApiConfiguration;

            var configuration = new Synesthesias.PLATEAU.Snap.Generated.Client.Configuration
            {
                BasePath = apiConfiguration.EndPoint,
                DefaultHeaders = { { apiConfiguration.ApiKeyType, apiConfiguration.ApiKeyValue } }
            };

            builder.RegisterInstance(configuration);
        }

        private static void ConfigureRepository(IContainerBuilder builder)
        {
            builder.Register<TextureRepository>(Lifetime.Singleton);
            builder.Register<ValidationRepository>(Lifetime.Singleton);
        }
    }
}