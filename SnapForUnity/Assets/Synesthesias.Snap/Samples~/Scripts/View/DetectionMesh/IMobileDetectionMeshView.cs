using System;
using System.Collections.Generic;
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
        /// メッシュのCollider
        /// </summary>
        MeshCollider MeshCollider { get; }

        /// <summary>
        /// GameObjectを取得する
        /// </summary>
        GameObject GetGameObject();

        /// <summary>
        /// 子GameObjectを取得する
        /// </summary>
        [Obsolete("削除予定")]
        GameObject GetChildGameObject();

        /// <summary>
        /// メッシュの頂点をピクセル座標で取得する
        /// </summary>
        /// <param name="camera">変換に使用するカメラ</param>
        /// <returns>WKT形式のPolygon文字列</returns>
        string GetVerticesAsScreenCoordinates(Camera camera);
    }
}