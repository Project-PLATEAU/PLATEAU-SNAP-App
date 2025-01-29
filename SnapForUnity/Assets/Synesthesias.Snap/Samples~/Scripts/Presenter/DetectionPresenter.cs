using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のPresenter
    /// </summary>
    public class DetectionPresenter : IAsyncStartable
    {
        private readonly DetectionModel model;
        private readonly DetectionView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionPresenter(
            DetectionModel model,
            DetectionView view)
        {
            this.model = model;
            this.view = view;
            OnSubscribe();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.Yield();
        }

        private void OnSubscribe()
        {
            view.BackButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickBack())
                .AddTo(view);
        }

        private void OnClickBack()
        {
            model.Back();
        }
    }
}