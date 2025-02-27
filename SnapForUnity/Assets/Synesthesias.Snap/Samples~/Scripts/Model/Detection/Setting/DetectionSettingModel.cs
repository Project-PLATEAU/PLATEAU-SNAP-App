using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using UnityEngine.Assertions;

namespace Synesthesias.Snap.Sample
{
    public class DetectionSettingModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly ReactiveProperty<bool> isGeospatialVisibleProperty = new(false);
        private readonly ReactiveProperty<bool> isManualDetectionProperty = new(true);
        private readonly ReactiveProperty<int> distanceProperty;
        private readonly int minimumDistance;
        private readonly int maximumDistance;
        private readonly int incrementDistance;
        private readonly DetectionMenuModel menuModel;

        /// <summary>
        /// Geospatial情報を表示するか
        /// </summary>
        public bool IsGeospatialVisible
            => isGeospatialVisibleProperty.Value;

        /// <summary>
        /// 手動検出か
        /// </summary>
        public bool IsManualDetection
            => isManualDetectionProperty.Value;

        /// <summary>
        /// 検出距離
        /// </summary>
        public int Distance
            => distanceProperty.Value;

        /// <summary>
        /// Geospatial情報を表示するかのObservable
        /// </summary>
        public Observable<bool> IsGeospatialVisibleAsObservable()
            => isGeospatialVisibleProperty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionSettingModel(
            DetectionMenuModel menuModel,
            int minimumDistance,
            int maximumDistance,
            int incrementDistance,
            int defaultDistance)
        {
            Assert.IsTrue(defaultDistance >= minimumDistance);
            Assert.IsTrue(defaultDistance <= maximumDistance);

            this.menuModel = menuModel;
            this.minimumDistance = minimumDistance;
            this.maximumDistance = maximumDistance;
            this.incrementDistance = incrementDistance;
            distanceProperty = new ReactiveProperty<int>(defaultDistance);
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
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            CreateMenu();
            await UniTask.Yield();
        }

        private void CreateMenu()
        {
            var geospatialVisibilityElementModel = CreateGeospatialVisibilityElementModel();
            menuModel.AddElement(geospatialVisibilityElementModel);

            var manualDetectionMenuElementModel = CreateIsManualDetectionMenuElementModel();
            menuModel.AddElement(manualDetectionMenuElementModel);
        }

        private DetectionMenuElementModel CreateGeospatialVisibilityElementModel()
        {
            var elementModel = new DetectionMenuElementModel(
                text: "Geospatial: ---",
                onClickAsync: OnClickGeospatialAsync);

            isGeospatialVisibleProperty
                .Subscribe(isVisible =>
                {
                    var text = isVisible ? "Geospatial: 表示" : "Geospatial: 非表示";
                    elementModel.TextProperty.Value = text;
                })
                .AddTo(disposable);

            return elementModel;
        }

        private DetectionMenuElementModel CreateIsManualDetectionMenuElementModel()
        {
            var elementModel = new DetectionMenuElementModel(
                text: "検出距離: ---",
                onClickAsync: OnClickDistanceAsync);

            distanceProperty
                .Subscribe(distance =>
                {
                    var text = $"検出距離: {distance}M";
                    elementModel.TextProperty.Value = text;
                })
                .AddTo(disposable);

            return elementModel;
        }

        private async UniTask OnClickGeospatialAsync(CancellationToken cancellationToken)
        {
            var isVisible = isGeospatialVisibleProperty.Value;
            isGeospatialVisibleProperty.Value = !isVisible;
            await UniTask.Yield();
        }

        private async UniTask OnClickDistanceAsync(CancellationToken cancellationToken)
        {
            distanceProperty.Value = (distanceProperty.Value + incrementDistance - minimumDistance)
                                     % (maximumDistance - minimumDistance + incrementDistance)
                                     + minimumDistance;

            await UniTask.Yield();
        }
    }
}