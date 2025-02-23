using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ(簡易版)の結果
    /// </summary>
    public class GeospatialMeshResult
    {
        /// <summary>
        /// メインループの結果の種類
        /// </summary>
        public readonly GeospatialMainLoopState MainLoopState;

        /// <summary>
        /// 精度の種類
        /// </summary>
        public readonly GeospatialAccuracyState AccuracyState;

        /// <summary>
        /// 結果の種類
        /// </summary>
        public readonly GeospatialMeshResultType ResultType;

        /// <summary>
        /// アンカーのTransform
        /// </summary>
        public readonly Transform AnchorTransform;

        /// <summary>
        /// Mesh
        /// </summary>
        public readonly Mesh Mesh;

        /// <summary>
        /// 成功かどうか
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialMeshResult(
            GeospatialMainLoopState mainLoopState,
            GeospatialAccuracyState accuracyState,
            GeospatialMeshResultType resultType = GeospatialMeshResultType.None,
            Transform anchorTransform = null,
            Mesh mesh = null)
        {
            MainLoopState = mainLoopState;
            AccuracyState = accuracyState;
            ResultType = resultType;
            AnchorTransform = anchorTransform;
            Mesh = mesh;

            IsSuccess = MainLoopState.IsReady
                        && AccuracyState == GeospatialAccuracyState.HighAccuracy
                        && ResultType == GeospatialMeshResultType.Success;
        }
    }
}