using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// Mockサーバーのみを動作させるためのLifetimeScope
    /// </summary>
    public class MockServerLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            ConfigureMockServer(builder);
        }

        private void ConfigureMockServer(IContainerBuilder builder)
        {
#if UNITY_EDITOR
            builder.Register<MockServerModel>(Lifetime.Singleton);
#endif
        }
    }
}