using Cysharp.Threading.Tasks;
using Synesthesias.Snap.Runtime;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのCullingのModel
    /// </summary>
    public class DetectionMeshCullingModel
    {
        private readonly MeshRepository meshRepository;
        private readonly IMeshValidationModel meshValidationModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMeshCullingModel(
            MeshRepository meshRepository,
            IMeshValidationModel meshValidationModel)
        {
            this.meshRepository = meshRepository;
            this.meshValidationModel = meshValidationModel;
        }

        /// <summary>
        /// メッシュのCulling
        /// </summary>
        public async UniTask CullingAsync(CancellationToken cancellationToken)
        {
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);

            var meshes = meshRepository.DetectedMeshViews
                .ToArray();

            foreach (var mesh in meshes)
            {
                await MeshCullingAsync(mesh, cancellationToken);
                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            }

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
        }

        private async UniTask MeshCullingAsync(
            IMobileDetectionMeshView meshView,
            CancellationToken cancellationToken)
        {
            if (meshView == null)
            {
                return;
            }

            var angleResultType = meshValidationModel.GetMeshValidationAngleResultType(
                meshTransform: meshView.MeshFilter.transform,
                mesh: meshView.MeshFilter.mesh);

            var vertexResultType = meshValidationModel.GetMeshValidationVertexResultType(
                meshTransform: meshView.MeshFilter.transform,
                mesh: meshView.MeshFilter.mesh);

            var isSuccess = angleResultType == MeshValidationAngleResultType.Valid
                            && vertexResultType == MeshValidationVertexResultType.Valid;

            meshView.MeshRenderer.enabled = isSuccess;
            meshView.MeshCollider.enabled = isSuccess;

            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
        }
    }
}