using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの検証
    /// </summary>
    public class MobileMeshValidationModel : IMeshValidationModel
    {
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly Camera camera;
        private readonly MeshValidationAngleThresholdModel angleThresholdModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileMeshValidationModel(
            GeospatialAccuracyModel accuracyModel,
            Camera camera,
            MeshValidationAngleThresholdModel angleThresholdModel)
        {
            this.accuracyModel = accuracyModel;
            this.camera = camera;
            this.angleThresholdModel = angleThresholdModel;
        }

        /// <summary>
        /// メッシュを検証します
        /// </summary>
        public MeshValidationResult Validate(
            Transform meshTransform,
            Mesh mesh)
        {
            var accuracyResult = accuracyModel.GetAccuracy();

            if (!accuracyResult.IsSuccess)
            {
                return new MeshValidationResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyResult: accuracyResult,
                    meshAngleResultType: MeshValidationAngleResultType.None,
                    meshVertexResultType: MeshValidationVertexResultType.None);
            }

            var angleResultType = GetMeshValidationAngleResultType(
                meshTransform: meshTransform,
                mesh: mesh);

            var vertexResultType = GetMeshValidationVertexResultType(
                meshTransform: meshTransform,
                mesh: mesh);

            return new MeshValidationResult(
                mainLoopState: accuracyResult.MainLoopState,
                accuracyResult: accuracyResult,
                angleResultType,
                vertexResultType);
        }

        /// <summary>
        /// メッシュの角度の検証結果の種類
        /// </summary>
        public MeshValidationAngleResultType GetMeshValidationAngleResultType(
            Transform meshTransform,
            Mesh mesh)
        {
            return MeshValidationAngleResultType.Valid;

            if (mesh.normals.Length < 1)
            {
                return MeshValidationAngleResultType.Invalid;
            }

            var meshNormal = meshTransform.TransformDirection(mesh.normals[0]).normalized;
            var toCamera = (camera.transform.position - meshTransform.position).normalized;
            var angle = Vector3.Angle(toCamera, meshNormal);

            var isValidAngle = angle >= angleThresholdModel.MinimumAngleThreshold
                               && angle <= angleThresholdModel.MaximumAngleThreshold;

            var result = isValidAngle
                ? MeshValidationAngleResultType.Valid
                : MeshValidationAngleResultType.Invalid;

            return result;
        }

        /// <summary>
        /// メッシュの頂点の検証結果の種類を取得
        /// </summary>
        public MeshValidationVertexResultType GetMeshValidationVertexResultType(
            Transform meshTransform,
            Mesh mesh)
        {
            return MeshValidationVertexResultType.Valid;

            var cameraPosition = camera.transform.position;

            foreach (var vertex in mesh.vertices)
            {
                var worldVertex = meshTransform.TransformPoint(vertex);

                var viewPortPoint = camera.WorldToViewportPoint(worldVertex);

                var isInsideCamera = viewPortPoint.x is >= 0 and <= 1
                                     && viewPortPoint.y is >= 0 and <= 1
                                     && viewPortPoint.z >= 0;

                if (!isInsideCamera)
                {
                    return MeshValidationVertexResultType.Invalid;
                }

                var direction = worldVertex - cameraPosition;

                if (!Physics.Raycast(cameraPosition, direction.normalized, out RaycastHit hit, direction.magnitude))
                {
                    continue;
                }

                if (hit.transform != meshTransform)
                {
                    return MeshValidationVertexResultType.Invalid;
                }
            }

            return MeshValidationVertexResultType.Valid;
        }
    }
}