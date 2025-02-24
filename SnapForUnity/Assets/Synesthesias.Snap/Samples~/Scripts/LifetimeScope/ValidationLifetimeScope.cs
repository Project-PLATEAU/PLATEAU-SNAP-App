using Synesthesias.PLATEAU.Snap.Generated.Api;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    public class ValidationLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private ValidationView validationView;
        [SerializeField] private ValidationDialogView dialogPrefab;
        [SerializeField] private Transform dialogTransform;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            ConfigureAPI(builder);
            ConfigureRepository(builder);
            ConfigureValidation(builder);
            ConfigureDialog(builder);
        }

        private void ConfigureAPI(IContainerBuilder builder)
        {
            var configuration = Parent.Container.Resolve<Synesthesias.PLATEAU.Snap.Generated.Client.Configuration>();
            var api = new ImagesApi(configuration: configuration);

            builder.RegisterInstance(api)
                .AsImplementedInterfaces();
        }

        private static void ConfigureRepository(IContainerBuilder builder)
        {
            builder.Register<ImageRepository>(Lifetime.Singleton);
        }

        private void ConfigureValidation(IContainerBuilder builder)
        {
            builder.RegisterInstance(validationView);
            builder.RegisterEntryPoint<ValidationPresenter>();

            var platformModel = Parent.Container.Resolve<PlatformModel>();

            if (platformModel.IsSupportedMobileDevice())
            {
                builder.Register<ValidationModel>(Lifetime.Singleton)
                    .AsImplementedInterfaces();

                return;
            }

            builder.Register<MockValidationModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private void ConfigureDialog(IContainerBuilder builder)
        {
            builder.Register<ValidationDialogModel>(Lifetime.Singleton);

            builder.RegisterComponentInNewPrefab(dialogPrefab, Lifetime.Singleton)
                .UnderTransform(dialogTransform);

            builder.RegisterEntryPoint<ValidationDialogPresenter>();
        }
    }
}