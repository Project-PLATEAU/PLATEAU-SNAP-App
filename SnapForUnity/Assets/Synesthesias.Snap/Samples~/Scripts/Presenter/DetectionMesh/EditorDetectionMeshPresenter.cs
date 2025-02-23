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
        private IList<ISurfaceModel> detectedSurfaces = new List<ISurfaceModel>();
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
            
        }
    }
}