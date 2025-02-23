using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュ(簡易版)のView(エディタ用)
    /// </summary>
    public class MobileDetectionSimpleMeshView : MonoBehaviour, IMobileDetectionMeshView
    {
        [SerializeField] private string id;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;

        /// <summary>
        /// メッシュのID
        /// </summary>
        public string Id
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// メッシュのFilter
        /// </summary>
        public MeshFilter MeshFilter
            => meshFilter;

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

        /// <summary>
        /// メッシュのCollider
        /// </summary>
        public MeshCollider MeshCollider
            => meshCollider;

        [Obsolete("削除予定")]
        public GameObject GetChildGameObject()
        {
            throw new NotImplementedException();
        }
    }
}