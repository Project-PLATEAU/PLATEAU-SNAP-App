using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのModel(エディタ)
    /// </summary>
    public class EditorMeshModel : IDisposable
    {
        private readonly List<GameObject> anchorObjects = new();
        private readonly EditorDetectionMeshView meshViewTemplate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorMeshModel(EditorDetectionMeshView meshViewTemplate)
        {
            this.meshViewTemplate = meshViewTemplate;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Clear();
        }

        /// <summary>
        /// メッシュをAFGeospatialアンカーの位置に生成する
        /// </summary>
        public EditorDetectionMeshView CreateMeshAtTransform(
            string id,
            Vector3 position,
            Quaternion rotation)
        {
            var view = Object.Instantiate(
                original: meshViewTemplate,
                position: position,
                rotation: rotation);

            view.Id = id;
            anchorObjects.Add(view.gameObject);
            return view;
        }

        /// <summary>
        /// 全てのメッシュを削除する
        /// </summary>
        public void Clear()
        {
            foreach (var anchorObject in anchorObjects)
            {
                Object.Destroy(anchorObject);
            }

            anchorObjects.Clear();
        }
    }
}