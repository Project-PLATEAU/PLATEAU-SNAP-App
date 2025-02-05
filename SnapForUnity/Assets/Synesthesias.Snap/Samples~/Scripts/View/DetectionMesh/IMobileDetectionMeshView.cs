using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    public interface IMobileDetectionMeshView
    {
        /// <summary>
        /// メッシュのRenderer
        /// </summary>
        MeshRenderer MeshRenderer { get; }

        /// <summary>
        /// GameObjectを取得する
        /// </summary>
        GameObject GetGameObject();
    }
}