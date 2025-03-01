using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 環境設定のScriptableObject
    /// </summary>
    [CreateAssetMenu(menuName = "Synesthesias/Snap/Sample/EnvironmentScriptableObject")]
    public class EnvironmentScriptableObject : ScriptableObject, IEnvironmentModel
    {
        [SerializeField] private EnvironmentType environmentType;
        [SerializeField] private ApiConfigurationScriptableObject apiConfiguration;
        [SerializeField] private DetectionMeshType detectionMeshType;

        /// <summary>
        /// 環境の種類
        /// </summary>
        public EnvironmentType EnvironmentType
            => environmentType;

        /// <summary>
        /// APIの設定
        /// </summary>
        public IApiConfigurationModel ApiConfiguration
            => apiConfiguration;

        /// <summary>
        /// 建物検出画面のメッシュの種類
        /// </summary>
        public DetectionMeshType DetectionMeshType
            => detectionMeshType;
    }
}