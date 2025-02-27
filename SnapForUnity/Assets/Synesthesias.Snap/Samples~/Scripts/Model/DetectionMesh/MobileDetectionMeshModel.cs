using Synesthesias.Snap.Runtime;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのModel(携帯端末)
    /// </summary>
    public class MobileDetectionMeshModel : IDisposable
    {
        private readonly List<GameObject> anchorObjects = new();
        private readonly IGeospatialMeshModel mobileGeospatialMeshModel;
        private readonly MobileDetectionMeshView meshViewTemplate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileDetectionMeshModel(
            IGeospatialMeshModel mobileGeospatialMeshModel,
            MobileDetectionMeshView meshViewTemplate)
        {
            this.mobileGeospatialMeshModel = mobileGeospatialMeshModel;
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
        /// メッシュ(surface)をAFGeospatialアンカーの位置に生成する
        /// </summary>
        public async UniTask<MobileDetectionMeshView> CreateMeshView(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var meshResult = await mobileGeospatialMeshModel.CreateMeshAsync(
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
            foreach (var anchorObject in anchorObjects
                         .Where(anchorObject => anchorObject))
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