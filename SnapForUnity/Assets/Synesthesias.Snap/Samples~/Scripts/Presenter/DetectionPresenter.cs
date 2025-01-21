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
        private readonly DetectionModel _model;
        private readonly DetectionView _view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionPresenter(
            DetectionModel model,
            DetectionView view)
        {
            _model = model;
            _view = view;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            OnSubscribe();
            await UniTask.Yield();
        }

        private void OnSubscribe()
        {
            _view.BackButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickBack())
                .AddTo(_view);
        }

        private void OnClickBack()
        {
            _model.Back();
        }
    }
}