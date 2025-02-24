using Cysharp.Threading.Tasks;
using Synesthesias.Snap.Runtime;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュ(簡易版)のModel(携帯端末)
    /// </summary>
    public class MobileDetectionSimpleMeshModel : IDisposable
    {
        private readonly List<GameObject> anchorObjects = new();
        private readonly MobileDetectionSimpleMeshView meshViewTemplate;
        private readonly IGeospatialSimpleMeshModel geospatialSimpleMeshModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileDetectionSimpleMeshModel(
            IGeospatialSimpleMeshModel geospatialSimpleMeshModel,
            MobileDetectionSimpleMeshView meshViewTemplate)
        {
            this.geospatialSimpleMeshModel = geospatialSimpleMeshModel;
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
        /// メッシュを作成する
        /// </summary>
        public async UniTask<MobileDetectionSimpleMeshView> CreateMeshView(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var meshResult = await geospatialSimpleMeshModel.CreateMeshAsync(
                camera: camera,
                surface: surface,
                eunRotation: eunRotation,
                cancellationToken: cancellationToken);

            if (meshResult.ResultType != GeospatialMeshResultType.Success)
            {
                return null;
            }

            var view = Object.Instantiate(
                meshViewTemplate,
                meshResult.AnchorTransform);

            anchorObjects.Add(view.gameObject);

            view.Id = surface.GmlId;
            view.MeshFilter.mesh = meshResult.Mesh;
            view.MeshCollider.sharedMesh = meshResult.Mesh;
            return view;
        }

        /// <summary>
        /// メッシュの表示を設定する
        /// </summary>
        public void SetMeshActive(bool isActive)
        {
            foreach (var anchorObject in anchorObjects)
            {
                anchorObject.SetActive(isActive);
            }
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