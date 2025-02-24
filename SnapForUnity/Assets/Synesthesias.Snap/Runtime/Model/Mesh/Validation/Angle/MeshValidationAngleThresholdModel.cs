namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの角度の検証しきい値のModel
    /// </summary>
    public class MeshValidationAngleThresholdModel
    {
        /// <summary>
        /// 最小角度のしきい値
        /// </summary>
        public readonly float MinimumAngleThreshold;

        /// <summary>
        /// 最大角度のしきい値
        /// </summary>
        public readonly float MaximumAngleThreshold;

        /// <summary>
        /// デフォルト
        /// </summary>
        public static MeshValidationAngleThresholdModel Default => new(
            minimumAngleThreshold: 0,
            maximumAngleThreshold: 180);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeshValidationAngleThresholdModel(
            float minimumAngleThreshold,
            float maximumAngleThreshold)
        {
            MinimumAngleThreshold = minimumAngleThreshold;
            MaximumAngleThreshold = maximumAngleThreshold;
        }
    }
}