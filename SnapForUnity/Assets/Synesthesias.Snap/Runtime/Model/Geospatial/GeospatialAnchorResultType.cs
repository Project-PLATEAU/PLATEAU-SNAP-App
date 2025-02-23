namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialのアンカーの結果の種類
    /// </summary>
    public enum GeospatialAnchorResultType
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 失敗
        /// </summary>
        Failed,

        /// <summary>
        /// 成功
        /// </summary>
        Success,
    }

    public static class GeospatialAnchorResultTypeExtensions
    {
        /// <summary>
        /// メッセージに変換する
        /// </summary>
        public static string ToMessage(this GeospatialAnchorResultType resultType)
        {
            var result = resultType switch
            {
                GeospatialAnchorResultType.None => "なし",
                GeospatialAnchorResultType.Failed => "アンカーの作成に失敗しました",
                GeospatialAnchorResultType.Success => "アンカーが作成されました",
                _ => throw new System.NotImplementedException($"未実装のGeospatialAnchorResultType: {resultType}")
            };

            return result;
        }
    }
}