using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using UnityEngine;
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
            try
            {
                await model.StartAsync(
                    camera: view.ArCamera,
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // キャンセレーション処理は正常な動作のため、ログ出力しない
                return;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
            }
        }

        private void OnSubscribe()
        {
            model.OnSelectedAsObservable()
                .Subscribe(OnOnSelected)
                .AddTo(view);

            view.TouchView.OnScreenInputAsObservable()
                .Subscribe(OnScreenInput)
                .AddTo(view);

            model.OnIsGeospatialVisibleAsObservable()
                .Subscribe(OnClickGeospatial)
                .AddTo(view);

            view.MenuButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickMenu())
                .AddTo(view);

            view.CameraButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickCameraAsync().Forget(Debug.LogException))
                .AddTo(view);

            view.VpsResetButton
                .OnClickAsObservable()
                .Subscribe(_ => OnClickVpsResetAsync().Forget(Debug.LogException))
                .AddTo(view);
        }

        private void OnOnSelected(bool isSelected)
        {
            view.CameraButton.interactable = isSelected;
        }

        private void OnScreenInput(Vector2 position)
        {
            model.TouchScreen(view.ArCamera, position);
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
            await model.CaptureAsync(
                camera: view.ArCamera,
                cancellationToken: cancellationToken);
        }

        private async UniTask OnClickVpsResetAsync()
        {
            // リセット中はVPSリセットボタン無効化
            var prevVpsInteractable = view.VpsResetButton ? view.VpsResetButton.interactable : false;
            if (view.VpsResetButton)
            {
                view.VpsResetButton.interactable = false;
            }

            var cancellationToken = view.GetCancellationTokenOnDestroy();

            try
            {
                await model.ResetVpsSessionAndMeshesAsync(cancellationToken);
 
                // 表示: リセット完了 → 数秒後に非表示
                if (view.ResetInfoText)
                {
                    view.ResetInfoText.gameObject.SetActive(true);
                    view.ResetInfoText.text = "リセット完了";
                    await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: cancellationToken);
                    view.ResetInfoText.gameObject.SetActive(false);
                }
            }
            finally
            {
                if (view.VpsResetButton)
                {
                    view.VpsResetButton.interactable = prevVpsInteractable;
                }
            }
        }
    }
}