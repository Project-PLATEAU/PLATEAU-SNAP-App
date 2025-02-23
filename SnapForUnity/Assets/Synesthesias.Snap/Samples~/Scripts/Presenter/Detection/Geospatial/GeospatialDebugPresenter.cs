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
    /// Geospatialのデバッグ用Presenter
    /// </summary>
    public class GeospatialDebugPresenter : IDisposable, IAsyncStartable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly GeospatialPoseModel poseModel;
        private readonly GeospatialDebugModel debugModel;
        private readonly GeospatialDebugView debugView;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialDebugPresenter(
            GeospatialPoseModel poseModel,
            GeospatialDebugModel debugModel,
            GeospatialDebugView debugView)
        {
            this.poseModel = poseModel;
            this.debugModel = debugModel;
            this.debugView = debugView;
            OnSubscribe();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await UniTask.Yield();
        }

        private void OnSubscribe()
        {
            debugView.UpdateAsObservable()
                .Subscribe(_ => OnUpdate())
                .AddTo(disposable);
        }

        private void OnUpdate()
        {
            var pose = poseModel.GetCameraPose();
            debugView.DebugText.text = debugModel.GetDebugText(pose);
        }
    }
}