namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの精度の状態
    /// </summary>
    public enum GeospatialAccuracyState
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 低精度
        /// </summary>
        LowAccuracy,

        /// <summary>
        /// 高精度
        /// </summary>
        HighAccuracy
    }

    /// <summary>
    /// GeospatialAccuracyStateの拡張メソッド
    /// </summary>
    public static class GeospatialAccuracyStateExtensions
    {
        /// <summary>
        /// メッセージに変換する
        /// </summary>
        public static string ToMessage(this GeospatialAccuracyState state)
        {
            var result = state switch
            {
                GeospatialAccuracyState.None => "なし",
                GeospatialAccuracyState.LowAccuracy => "低精度",
                GeospatialAccuracyState.HighAccuracy => "高精度",
                _ => throw new System.NotImplementedException($"未実装のGeospatialAccuracyState: {state}")
            };

            return result;
        }
    }
}