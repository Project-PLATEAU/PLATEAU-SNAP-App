using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のPresenter(携帯端末)
    /// </summary>
    public class MobileDetectionPresenter : IAsyncStartable
    {
        private readonly MobileDetectionModel model;
        private readonly MobileDetectionView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileDetectionPresenter(
            MobileDetectionModel model,
            MobileDetectionView view)
        {
            this.model = model;
            this.view = view;
            OnSubscribe();
            view.CameraButton.interactable = false;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await model.StartAsync(cancellationToken);

            while (true)
            {
                var isDetect = await model.IsDetectAsync(cancellationToken);
                view.CameraButton.interactable = isDetect;
                await UniTask.WaitForSeconds(0.5F, cancellationToken: cancellationToken);
            }
        }

        private void OnSubscribe()
        {
            model.IsGeospatialVisibleProperty
                .Subscribe(OnClickGeospatial)
                .AddTo(view);

            view.MenuButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickMenu())
                .AddTo(view);

            view.CameraButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickCameraAsync().Forget())
                .AddTo(view);
        }

        private void OnClickMenu()
        {
            model.ShowMenu();
        }

        private void OnClickGeospatial(bool isVisible)
        {
            view.GeospatialObject.SetActive(isVisible);
        }

        private async UniTask OnClickCameraAsync()
        {
            view.CameraButton.interactable = false;
            var cancellationToken = view.GetCancellationTokenOnDestroy();
            await model.CaptureAsync(cancellationToken);
        }
    }
}