using System;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// 建物検出画面のメッシュのView
    /// </summary>
    public class MeshView : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;

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
    }
}