using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ(簡易版)のModel(エディタ)
    /// Hullのみ対応(Holeは無視される)
    /// </summary>
    public class EditorGeospatialSimpleMeshModel : IGeospatialSimpleMeshModel
    {
        private readonly SimpleMeshModel simpleMeshModel;
        private readonly IGeospatialMathModel geospatialMathModel;
        private readonly GeospatialMainLoopState mainLoopState = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorGeospatialSimpleMeshModel(
            SimpleMeshModel simpleMeshModel,
            IGeospatialMathModel geospatialMathModel)
        {
            this.simpleMeshModel = simpleMeshModel;
            this.geospatialMathModel = geospatialMathModel;
            mainLoopState.SetStateType(GeospatialMainLoopStateType.Ready);
        }

        /// <summary>
        /// メッシュの生成
        /// </summary>
        public async UniTask<GeospatialMeshResult> CreateMeshAsync(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            if (surface?.Coordinates == null || surface.Coordinates.Count < 1)
            {
                return new GeospatialMeshResult(
                    mainLoopState: mainLoopState,
                    accuracyState: GeospatialAccuracyState.HighAccuracy,
                    GeospatialMeshResultType.EmptyCoordinate);
            }

            // Hull(面)のみ対応(Holeは無視)
            var hullCoordinates = surface.Coordinates[0];

            var originPose = CreatePose(
                coordinates: hullCoordinates[0],
                eunRotation: eunRotation);

            var vertices = CreateVertices(
                originPosition: originPose.position,
                coordinates: hullCoordinates,
                eunRotation: eunRotation);

            if (!simpleMeshModel.TryCreateFanTriangles(
                    cameraPosition: camera.transform.position,
                    vertices: vertices,
                    results: out var triangles))
            {
                return new GeospatialMeshResult(
                    mainLoopState: mainLoopState,
                    accuracyState: GeospatialAccuracyState.HighAccuracy,
                    resultType: GeospatialMeshResultType.InsufficientVertices);
            }

            var mesh = simpleMeshModel.CreateMesh(vertices, triangles);

            var anchorObject = new GameObject(
                name: "EditorAnchor") { transform = { position = originPose.position } };

            return new GeospatialMeshResult(
                mainLoopState: mainLoopState,
                accuracyState: GeospatialAccuracyState.HighAccuracy,
                resultType: GeospatialMeshResultType.Success,
                anchorTransform: anchorObject.transform,
                mesh: mesh);
        }

        private Vector3[] CreateVertices(
            Vector3 originPosition,
            List<List<double>> coordinates,
            Quaternion eunRotation)
        {
            // 座標の数だけPoseを作成
            var poses = coordinates
                .Skip(1)
                .Select(coordinate =>
                {
                    var pose = CreatePose(coordinate, eunRotation);
                    return pose;
                }).ToArray();

            var vertices = poses
                .Select(pose => pose.position - originPosition)
                .ToArray();

            var results = new[] { Vector3.zero }
                .Concat(vertices)
                .ToArray();

            return results;
        }

        private Pose CreatePose(
            List<double> coordinates,
            Quaternion eunRotation)
        {
            var geospatialPose = geospatialMathModel.CreateGeospatialPose(
                latitude: coordinates[0],
                longitude: coordinates[1],
                altitude: coordinates[2],
                eunRotation: eunRotation);

            var pose = geospatialMathModel.CreatePose(geospatialPose);
            return pose;
        }
    }
}