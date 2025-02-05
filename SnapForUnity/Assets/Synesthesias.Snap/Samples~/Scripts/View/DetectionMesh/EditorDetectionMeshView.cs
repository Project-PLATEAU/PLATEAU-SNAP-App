using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのView(エディタ用)
    /// </summary>
    public class EditorDetectionMeshView : MonoBehaviour, IMobileDetectionMeshView
    {
        [SerializeField] private MeshRenderer meshRenderer;

        /// <summary>
        /// メッシュのRenderer
        /// </summary>
        public MeshRenderer MeshRenderer
            => meshRenderer;

        /// <summary>
        /// GameObjectを取得する
        /// </summary>
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}