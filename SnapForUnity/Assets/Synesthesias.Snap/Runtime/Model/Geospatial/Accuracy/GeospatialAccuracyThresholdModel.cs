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
        /// デフォルト
        /// </summary>
        public static GeospatialAccuracyThresholdModel Default => new(
            headingThreshold: 25,
            horizontalAccuracyThreshold: 20);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAccuracyThresholdModel(double headingThreshold, double horizontalAccuracyThreshold)
        {
            HeadingThreshold = headingThreshold;
            HorizontalAccuracyThreshold = horizontalAccuracyThreshold;
        }
    }
}