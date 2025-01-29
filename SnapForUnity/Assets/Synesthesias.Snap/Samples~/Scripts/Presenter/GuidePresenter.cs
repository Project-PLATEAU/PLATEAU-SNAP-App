using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 利用ガイド画面のPresenter
    /// </summary>
    public class GuidePresenter : IAsyncStartable
    {
        private readonly GuideModel guideModel;
        private readonly GuideView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GuidePresenter(
            GuideModel guideModel,
            GuideView view)
        {
            this.guideModel = guideModel;
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
            view.CloseButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickClose())
                .AddTo(view);
        }

        private void OnClickClose()
        {
            guideModel.Close();
        }
    }
}