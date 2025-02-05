using Cysharp.Threading.Tasks;
using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのModel(携帯端末)
    /// </summary>
    public class MobileMeshModel : IDisposable
    {
        private readonly List<GameObject> anchorObjects = new();
        private readonly GeospatialAsyncModel geospatialAsyncModel;
        private readonly MobileDetectionMeshView meshViewTemplate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileMeshModel(
            GeospatialAsyncModel geospatialAsyncModel,
            MobileDetectionMeshView meshViewTemplate)
        {
            this.geospatialAsyncModel = geospatialAsyncModel;
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
        public MobileDetectionMeshView CreateMeshAtARGeospatialAnchor(ARGeospatialAnchor geospatialAnchor)
        {
            anchorObjects.Add(geospatialAnchor.gameObject);
            var view = Object.Instantiate(meshViewTemplate, geospatialAnchor.transform);
            anchorObjects.Add(view.gameObject);
            return view;
        }

        /// <summary>
        /// メッシュをARアンカーの位置に生成する
        /// </summary>
        public MobileDetectionMeshView CreateMeshAtARAnchor(ARAnchor arAnchor)
        {
            anchorObjects.Add(arAnchor.gameObject);
            var view = Object.Instantiate(meshViewTemplate, arAnchor.transform);
            anchorObjects.Add(view.gameObject);
            return view;
        }

        /// <summary>
        /// GeospatialPoseの位置にメッシュを生成する
        /// </summary>
        public async UniTask<MobileDetectionMeshView> CreateMeshAtARGeospatialPose(
            GeospatialPose geospatialPose,
            CancellationToken cancellationToken)
        {
            var arGeospatialAnchor = await geospatialAsyncModel.CreateARGeospatialAnchorAsync(
                geospatialPose,
                cancellationToken);

            var view = CreateMeshAtARGeospatialAnchor(arGeospatialAnchor);
            return view;
        }

        /// <summary>
        /// カメラの位置にメッシュを生成する
        /// </summary>
        public async UniTask<MobileDetectionMeshView> CreateMeshAtCameraAsync(CancellationToken cancellationToken)
        {
            var geospatialPose = geospatialAsyncModel.GetCameraGeospatialPose();
            var view = await CreateMeshAtARGeospatialPose(geospatialPose, cancellationToken);
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