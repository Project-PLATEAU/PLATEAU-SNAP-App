namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの精度の結果
    /// </summary>
    public class GeospatialAccuracyResult
    {
        /// <summary>
        /// メインループの状態
        /// </summary>
        public readonly GeospatialMainLoopState MainLoopState;

        /// <summary>
        /// 精度の状態
        /// </summary>
        public readonly GeospatialAccuracyState AccuracyState;

        /// <summary>
        /// 成功かどうか
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAccuracyResult(
            GeospatialMainLoopState mainLoopState,
            GeospatialAccuracyState accuracyState = GeospatialAccuracyState.None)
        {
            MainLoopState = mainLoopState;
            AccuracyState = accuracyState;

            IsSuccess = MainLoopState.IsReady
                        && AccuracyState == GeospatialAccuracyState.HighAccuracy;
        }
    }
}