using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// ブート画面のPresenter
    /// </summary>
    public class BootPresenter : IAsyncStartable
    {
        private readonly ResidentView view;
        private readonly SceneModel sceneModel;
        private readonly BootModel bootModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BootPresenter(
            SceneModel sceneModel,
            BootModel bootModel,
            ResidentView view)
        {
            this.sceneModel = sceneModel;
            this.bootModel = bootModel;
            this.view = view;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await bootModel.InitializeAsync(cancellationToken);
            sceneModel.Transition(SceneNameDefine.Main);
        }
    }
}