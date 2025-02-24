namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの角度の検証結果の種類
    /// </summary>
    public enum MeshValidationAngleResultType
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 無効な角度
        /// カメラの許容角度を超えている
        /// </summary>
        Invalid,

        /// <summary>
        /// 有効な角度
        /// </summary>
        Valid
    }

    /// <summary>
    /// メッシュの角度の検証結果の種類の拡張メソッド
    /// </summary>
    public static class MeshValidationAngleResultTypeExtensions
    {
        /// <summary>
        /// メッセージに変換する
        /// </summary>
        public static string ToMessage(this MeshValidationAngleResultType resultType)
        {
            var result = resultType switch
            {
                MeshValidationAngleResultType.None => "なし",
                MeshValidationAngleResultType.Invalid => "カメラの許容角度を超えています",
                MeshValidationAngleResultType.Valid => "有効な角度です",
                _ => throw new System.NotImplementedException($"未実装のMeshValidationAngleResultType: {resultType}")
            };

            return result;
        }
    }
}