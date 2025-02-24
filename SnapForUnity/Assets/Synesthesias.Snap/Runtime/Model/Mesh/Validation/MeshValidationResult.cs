namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの検証結果
    /// </summary>
    public class MeshValidationResult
    {
        /// <summary>
        /// メインループの状態
        /// </summary>
        public readonly GeospatialMainLoopState MainLoopState;

        /// <summary>
        /// Geospatialの精度の結果
        /// </summary>
        public readonly GeospatialAccuracyResult AccuracyResult;

        /// <summary>
        /// メッシュの角度の検証結果の種類
        /// </summary>
        public readonly MeshValidationAngleResultType MeshAngleResultType;

        /// <summary>
        /// メッシュの頂点の検証結果の種類
        /// </summary>
        public readonly MeshValidationVertexResultType MeshVertexResultType;

        /// <summary>
        /// 成功かどうか
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeshValidationResult(
            GeospatialMainLoopState mainLoopState,
            GeospatialAccuracyResult accuracyResult,
            MeshValidationAngleResultType meshAngleResultType,
            MeshValidationVertexResultType meshVertexResultType)
        {
            MainLoopState = mainLoopState;
            AccuracyResult = accuracyResult;
            MeshAngleResultType = meshAngleResultType;
            MeshVertexResultType = meshVertexResultType;

            IsSuccess = mainLoopState.IsReady
                        && accuracyResult.IsSuccess
                        && meshAngleResultType == MeshValidationAngleResultType.Valid
                        && meshVertexResultType == MeshValidationVertexResultType.Valid;
        }
    }
}