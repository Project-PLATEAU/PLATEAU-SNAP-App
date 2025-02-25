using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    public class DetectionSettingModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly int minimumDistance;
        private readonly int maximumDistance;
        private readonly int incrementDistance;
        private readonly DetectionMenuModel menuModel;

        /// <summary>
        /// Geospatial情報を表示するか
        /// </summary>
        public readonly ReactiveProperty<bool> IsGeospatialVisibleProperty = new(true);

        /// <summary>
        /// 手動検出か
        /// </summary>
        public readonly ReactiveProperty<bool> IsManualDetectionProperty = new(true);

        /// <summary>
        /// 検出距離
        /// </summary>
        public readonly ReactiveProperty<int> DistanceProperty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionSettingModel(
            DetectionMenuModel menuModel,
            int minimumDistance,
            int maximumDistance,
            int incrementDistance)
        {
            this.menuModel = menuModel;
            this.minimumDistance = minimumDistance;
            this.maximumDistance = maximumDistance;
            this.incrementDistance = incrementDistance;
            DistanceProperty = new ReactiveProperty<int>(minimumDistance);
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

            IsGeospatialVisibleProperty
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

            DistanceProperty
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
            var isVisible = IsGeospatialVisibleProperty.Value;
            IsGeospatialVisibleProperty.Value = !isVisible;
            await UniTask.Yield();
        }

        private async UniTask OnClickDistanceAsync(CancellationToken cancellationToken)
        {
            DistanceProperty.Value = (DistanceProperty.Value + incrementDistance - minimumDistance)
                                     % (maximumDistance - minimumDistance + incrementDistance)
                                     + minimumDistance;

            await UniTask.Yield();
        }
    }
}