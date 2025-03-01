namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物の検出のメッシュの種類
    /// </summary>
    public enum DetectionMeshType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None,

        /// <summary>
        /// 簡易なMeshを使用
        /// </summary>
        Simple,

        /// <summary>
        /// Unity.iShapeを使用
        /// </summary>
        iShape,
    }
}