using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Synesthesias.Snap.Runtime;
using System;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// GeospatialのメインループのPresenter
    /// </summary>
    public class GeospatialMainLoopPresenter : IDisposable, IAsyncStartable
    {
        private readonly CompositeDisposable disposables = new();
        private readonly GeospatialMainLoopModel model;
        private readonly GeospatialMainLoopView view;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialMainLoopPresenter(
            GeospatialMainLoopModel model,
            GeospatialMainLoopView view)
        {
            this.model = model;
            this.view = view;
            OnSubscribe();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await model.EnableAsync(cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    await model.MainLoopAsync(cancellationToken);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
            }
            finally
            {
                model.Disable();
            }
        }

        private void OnSubscribe()
        {
            view.AwakeObservable()
                .Subscribe(_ => OnAwake())
                .AddTo(disposables);

            view.OnDisableAsObservable()
                .Subscribe(_ => model.Disable())
                .AddTo(disposables);
        }

        private void OnAwake()
        {
            if (view.IsLockPortrait)
            {
                GeospatialMainLoopModel.LockToPortrait();
            }

            if (view.IsFrameRateAs60)
            {
                GeospatialMainLoopModel.SetFrameRateAs60();
            }
        }
    }
}