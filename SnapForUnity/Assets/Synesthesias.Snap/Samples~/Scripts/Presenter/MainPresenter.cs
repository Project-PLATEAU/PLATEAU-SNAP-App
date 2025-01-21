using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// メイン画面のPresenter
    /// </summary>
    public class MainPresenter : IAsyncStartable
    {
        private readonly MainModel model;
        private readonly MainView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPresenter(
            MainModel model,
            MainView view)
        {
            this.model = model;
            this.view = view;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            OnSubscribe();
            await UniTask.Yield();
        }

        private void OnSubscribe()
        {
            view.StartButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickStart())
                .AddTo(view);
        }

        private void OnClickStart()
        {
            model.Start();
        }
    }
}