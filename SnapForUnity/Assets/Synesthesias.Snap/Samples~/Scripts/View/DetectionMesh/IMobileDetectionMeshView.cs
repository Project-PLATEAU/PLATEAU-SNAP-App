using System;
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
        /// メッシュのFilter
        /// </summary>
        MeshFilter MeshFilter { get; }

        /// <summary>
        /// メッシュのRenderer
        /// </summary>
        MeshRenderer MeshRenderer { get; }

        /// <summary>
        /// GameObjectを取得する
        /// </summary>
        GameObject GetGameObject();

        /// <summary>
        /// 子GameObjectを取得する
        /// </summary>
        [Obsolete("削除予定")]
        GameObject GetChildGameObject();
    }
}