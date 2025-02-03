using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    public class BootLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<BootModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootPresenter>();
        }
    }
}