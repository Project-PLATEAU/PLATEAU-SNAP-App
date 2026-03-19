namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの精度のしきい値のModel
    /// </summary>
    public class GeospatialAccuracyThresholdModel
    {
        /// <summary>
        /// 方位角のしきい値
        /// </summary>
        public readonly double HeadingThreshold;

        /// <summary>
        /// 水平精度のしきい値
        /// </summary>
        public readonly double HorizontalAccuracyThreshold;

        /// <summary>
        /// 垂直精度のしきい値
        /// </summary>
        public readonly double VerticalAccuracyThreshold;

        /// <summary>
        /// デフォルト
        /// </summary>
        public static GeospatialAccuracyThresholdModel Default => new(
            headingThreshold: 5,
            horizontalAccuracyThreshold: 1,
            verticalAccuracyThreshold: 1);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAccuracyThresholdModel(double headingThreshold, double horizontalAccuracyThreshold, double verticalAccuracyThreshold)
        {
            HeadingThreshold = headingThreshold;
            HorizontalAccuracyThreshold = horizontalAccuracyThreshold;
            VerticalAccuracyThreshold = verticalAccuracyThreshold;
        }
    }
}