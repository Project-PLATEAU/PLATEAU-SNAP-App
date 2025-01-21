using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// ライフタイムスコープの基底クラス
    /// </summary>
    public abstract class BaseLifetimeScope : LifetimeScope
    {
        /// <summary>
        /// 起動時の処理
        /// </summary>
        protected virtual void OnBootstrap(IObjectResolver container)
        {
        }

        /// <summary>
        /// DIの設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            var sceneModel = Parent.Container.Resolve<SceneModel>();
            sceneModel.NotifyBoot();

            if (sceneModel.IsBootstrap())
            {
                OnBootstrap(Parent.Container);
            }
        }
    }
}