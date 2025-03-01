namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 環境のModel
    /// </summary>
    public interface IEnvironmentModel
    {
        /// <summary>
        /// 環境の種類
        /// </summary>
        EnvironmentType EnvironmentType { get; }

        /// <summary>
        /// APIの設定Model
        /// </summary>
        IApiConfigurationModel ApiConfiguration { get; }

        /// <summary>
        /// 建物検出画面のメッシュの種類
        /// </summary>
        DetectionMeshType DetectionMeshType { get; }
    }
}