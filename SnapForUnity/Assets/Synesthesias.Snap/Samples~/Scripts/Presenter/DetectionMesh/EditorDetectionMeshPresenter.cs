using Cysharp.Threading.Tasks;
using Synesthesias.Snap.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのPresenter(エディタ)
    /// </summary>
    public class DetectionMeshPresenter : IAsyncStartable
    {
        private const float SphereRadius = 0.5f;
        private IList<Surface> detectedSurfaces = new List<Surface>();
        private readonly EditorDetectionView detectionView;
        private readonly MeshView meshViewTemplate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMeshPresenter(
            EditorDetectionView detectionView,
            MeshView meshViewTemplate)
        {
            this.detectionView = detectionView;
            this.meshViewTemplate = meshViewTemplate;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Repository経由でAPI通信して取得する
            var content = JsonParser.Parse();
            detectedSurfaces = content.DetectedSurfaces;

            for (int index = 0; index < detectedSurfaces.Count(); index++)
            {
                LatLonTests.TestData[index].hull = detectedSurfaces[index].Coordinates[0]
                    .Take(detectedSurfaces[index].Coordinates[0].Length - 1).ToArray();
                LatLonTests.TestData[index].holes = null;

                var gmlId = detectedSurfaces[index].GmlId;
                var model = new MeshModel();
                var view = Object.Instantiate(meshViewTemplate, detectionView.MeshParent);
                var mesh = model.CreateMesh(index, gmlId);
                view.MeshFilter.mesh = mesh;
                model.CreateCollider(gmlId, mesh, view.transform);
            }
        }
    }
}