using Google.XR.ARCoreExtensions;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// GeospatialAnchorの結果
    /// </summary>
    public class GeospatialAnchorResult
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
        /// 結果の種類
        /// </summary>
        public readonly GeospatialAnchorResultType ResultType;

        /// <summary>
        /// 結果の値
        /// </summary>
        public readonly ARGeospatialAnchor Anchor;

        /// <summary>
        /// 成功かどうか
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAnchorResult(
            GeospatialMainLoopState mainLoopState,
            GeospatialAccuracyState accuracyState,
            GeospatialAnchorResultType resultType = GeospatialAnchorResultType.None,
            ARGeospatialAnchor anchor = null)
        {
            MainLoopState = mainLoopState;
            AccuracyState = accuracyState;
            ResultType = resultType;
            Anchor = anchor;

            IsSuccess = MainLoopState.IsReady
                        && AccuracyState == GeospatialAccuracyState.HighAccuracy
                        && ResultType == GeospatialAnchorResultType.Success;
        }
    }
}