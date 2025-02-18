using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    public interface IMobileDetectionMeshView
    {
        /// <summary>
        /// メッシュのID
        /// </summary>
        string Id { get; }

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