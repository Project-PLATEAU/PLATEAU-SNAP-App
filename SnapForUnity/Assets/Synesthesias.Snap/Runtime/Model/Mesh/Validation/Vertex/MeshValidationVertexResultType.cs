namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの頂点の検証結果の種類
    /// </summary>
    public enum MeshValidationVertexResultType
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 無効な頂点
        /// 頂点がカメラの画角外にある
        /// </summary>
        Invalid,

        /// <summary>
        /// 有効な頂点
        /// 全ての頂点がカメラの画角内にある
        /// </summary>
        Valid
    }

    /// <summary>
    /// メッシュの頂点の検証結果の種類の拡張メソッド
    /// </summary>
    public static class MeshValidationVertexResultTypeExtensions
    {
        /// <summary>
        /// メッセージに変換する
        /// </summary>
        public static string ToMessage(this MeshValidationVertexResultType resultType)
        {
            var result = resultType switch
            {
                MeshValidationVertexResultType.None => "なし",
                MeshValidationVertexResultType.Invalid => "頂点がカメラの画角外にあります",
                MeshValidationVertexResultType.Valid => "全ての頂点がカメラの画角内にあります",
                _ => throw new System.NotImplementedException($"未実装のMeshValidationVertexResultType: {resultType}")
            };

            return result;
        }
    }
}