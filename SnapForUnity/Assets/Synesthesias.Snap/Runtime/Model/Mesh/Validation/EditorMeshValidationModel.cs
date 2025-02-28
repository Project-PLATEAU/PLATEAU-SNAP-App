using Google.XR.ARCoreExtensions;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの検証
    /// </summary>
    public class EditorMeshValidationModel : IMeshValidationModel
    {
        private readonly Camera camera;
        private readonly MeshValidationAngleThresholdModel angleThresholdModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorMeshValidationModel(
            Camera camera,
            MeshValidationAngleThresholdModel angleThresholdModel)
        {
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
            var angleResultType = GetMeshValidationAngleResultType(
                meshTransform: meshTransform,
                mesh: mesh);

            var vertexResultType = GetMeshValidationVertexResultType(
                meshTransform: meshTransform,
                mesh: mesh);

            var mainLoopState = new GeospatialMainLoopState();
            mainLoopState.SetStateType(GeospatialMainLoopStateType.Ready);
            mainLoopState.SetEarthState(EarthState.Enabled);
            mainLoopState.SetFeatureSupported(FeatureSupported.Supported);

            var accuracyResult = new GeospatialAccuracyResult(
                mainLoopState: mainLoopState,
                GeospatialAccuracyState.HighAccuracy);

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
            var cameraPosition = camera.transform.position;

            foreach (var vertex in mesh.vertices)
            {
                var worldVertex = meshTransform.TransformPoint(vertex);
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