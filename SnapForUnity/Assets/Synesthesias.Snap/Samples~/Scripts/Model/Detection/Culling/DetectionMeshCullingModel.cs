using Cysharp.Threading.Tasks;
using Synesthesias.Snap.Runtime;
using System.Linq;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのCullingのModel
    /// </summary>
    public class DetectionMeshCullingModel
    {
        private readonly MeshRepository meshRepository;
        private readonly DetectionMaterialModel materialModel;
        private readonly IMeshValidationModel meshValidationModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMeshCullingModel(
            MeshRepository meshRepository,
            DetectionMaterialModel materialModel,
            IMeshValidationModel meshValidationModel)
        {
            this.meshRepository = meshRepository;
            this.materialModel = materialModel;
            this.meshValidationModel = meshValidationModel;
        }

        /// <summary>
        /// メッシュのCulling
        /// </summary>
        public async UniTask CullingAsync(CancellationToken cancellationToken)
        {
            var meshes = meshRepository.DetectedMeshViews
                .ToArray();

            foreach (var mesh in meshes)
            {
                MeshCulling(mesh);
            }

            await UniTask.DelayFrame(2, cancellationToken: cancellationToken);
        }

        private void MeshCulling(IMobileDetectionMeshView meshView)
        {
            if (meshView == null)
            {
                return;
            }

            var selectedMeshView = meshRepository.SelectedMeshViewProperty.Value;

            if (selectedMeshView != null && selectedMeshView.Id == meshView.Id)
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

            meshView.MeshRenderer.material = isSuccess
                ? materialModel.SelectableMaterial
                : materialModel.DetectedMaterial;

            meshView.MeshCollider.enabled = isSuccess;
        }
    }
}