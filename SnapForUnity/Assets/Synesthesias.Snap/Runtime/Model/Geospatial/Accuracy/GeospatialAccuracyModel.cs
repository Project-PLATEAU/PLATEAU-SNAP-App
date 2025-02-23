using Google.XR.ARCoreExtensions;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの精度を取得するModel
    /// </summary>
    public class GeospatialAccuracyModel
    {
        private readonly AREarthManager earthManager;
        private readonly GeospatialMainLoopModel mainLoopModel;
        private readonly GeospatialAccuracyThresholdModel thresholdModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAccuracyModel(
            AREarthManager earthManager,
            GeospatialMainLoopModel mainLoopModel,
            GeospatialAccuracyThresholdModel thresholdModel)
        {
            this.earthManager = earthManager;
            this.mainLoopModel = mainLoopModel;
            this.thresholdModel = thresholdModel;
        }

        /// <summary>
        /// 精度を取得する
        /// </summary>
        public GeospatialAccuracyResult GetAccuracy()
        {
            if (!mainLoopModel.State.IsReady)
            {
                return new GeospatialAccuracyResult(
                    mainLoopState: mainLoopModel.State);
            }

            var cameraGeospatialPose = earthManager.CameraGeospatialPose;

            if (cameraGeospatialPose.OrientationYawAccuracy > thresholdModel.HeadingThreshold
                || cameraGeospatialPose.HorizontalAccuracy > thresholdModel.HorizontalAccuracyThreshold)
            {
                return new GeospatialAccuracyResult(
                    mainLoopState: mainLoopModel.State,
                    accuracyState: GeospatialAccuracyState.LowAccuracy);
            }

            return new GeospatialAccuracyResult(
                mainLoopState: mainLoopModel.State,
                accuracyState: GeospatialAccuracyState.HighAccuracy);
        }
    }
}