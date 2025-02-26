using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのView(エディタ用)
    /// </summary>
    public class EditorDetectionMeshView : MonoBehaviour, IMobileDetectionMeshView
    {
        [SerializeField] private string id;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private Material debugSphereMaterial;

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

        public Material DebugSphereMaterial
            => debugSphereMaterial;

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

        // <summary>
        /// 子GameObjectを取得する
        /// </summary>
        public GameObject GetChildGameObject()
        {
            return gameObject.transform.GetChild(0).gameObject;
        }
    }
}