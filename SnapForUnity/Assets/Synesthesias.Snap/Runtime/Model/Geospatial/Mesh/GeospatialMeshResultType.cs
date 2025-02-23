using System;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ(簡易版)の結果の種類
    /// </summary>
    public enum GeospatialMeshResultType
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 座標が空
        /// </summary>
        EmptyCoordinate,

        /// <summary>
        /// 原点の作成に失敗
        /// </summary>
        OriginCreationFailed,

        /// <summary>
        /// アンカーの作成に失敗
        /// </summary>
        AnchorCreationFailed,

        /// <summary>
        /// 頂点が不足
        /// </summary>
        InsufficientVertices,

        /// <summary>
        /// 作成成功
        /// </summary>
        Success
    }

    /// <summary>
    /// SimpleMeshResultTypeの拡張メソッド
    /// </summary>
    public static class SimpleMeshResultTypeExtensions
    {
        /// <summary>
        /// メッセージに変換する
        /// </summary>
        public static string ToMessage(this GeospatialMeshResultType resultType)
        {
            var result = resultType switch
            {
                GeospatialMeshResultType.None => "なし",
                GeospatialMeshResultType.EmptyCoordinate => "座標が空のため、メッシュを作成できません",
                GeospatialMeshResultType.OriginCreationFailed => "原点の作成に失敗しました",
                GeospatialMeshResultType.AnchorCreationFailed => "アンカーの作成に失敗しました",
                GeospatialMeshResultType.InsufficientVertices => "頂点が不足しているため、メッシュを作成できません",
                GeospatialMeshResultType.Success => "メッシュの作成に成功しました",
                _ => throw new NotImplementedException($"未実装のSimpleMeshResultType: {resultType}")
            };

            return result;
        }
    }
}