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
    public class EditorGeospatialMeshModel : IGeospatialMeshModel
    {
        private readonly IGeospatialMathModel geospatialMathModel;
        private readonly IMeshFactoryModel meshFactoryModel;
        private readonly GeospatialMainLoopState mainLoopState = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorGeospatialMeshModel(
            IGeospatialMathModel geospatialMathModel,
            IMeshFactoryModel meshFactoryModel)
        {
            this.geospatialMathModel = geospatialMathModel;
            this.meshFactoryModel = meshFactoryModel;
            mainLoopState.SetStateType(GeospatialMainLoopStateType.Ready);
        }

        /// <summary>
        /// メッシュの生成
        /// </summary>
        public async UniTask<GeospatialMeshResult> CreateMeshAsync(
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var coordinates = surface?.GetUniqueCoordinates();

            if (coordinates == null || coordinates.Count < 1)
            {
                return new GeospatialMeshResult(
                    mainLoopState: mainLoopState,
                    accuracyState: GeospatialAccuracyState.HighAccuracy,
                    GeospatialMeshResultType.EmptyCoordinate);
            }

            var hullCoordinates = coordinates[0];
            var holesCoordinates = coordinates.Skip(1);

            var originPose = CreatePose(
                coordinates: hullCoordinates[0],
                eunRotation: eunRotation);

            var hullVertices = CreateVertices(
                originPosition: originPose.position,
                coordinates: hullCoordinates,
                eunRotation: eunRotation);

            var holesVertices = holesCoordinates.Select(
                coordinates => CreateVertices(
                    originPosition: originPose.position,
                    coordinates: coordinates,
                    eunRotation: eunRotation
                )).ToArray();

            var anchorObject = new GameObject(
                name: "EditorAnchor") { transform = { position = Vector3.zero } };

            var mesh = await meshFactoryModel.CreateAsync(
                hull: hullVertices,
                holes: holesVertices,
                cancellationToken: cancellationToken);

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
                .Select(coordinate =>
                {
                    var pose = CreatePose(coordinate, eunRotation);
                    return pose;
                }).ToArray();

            var vertices = poses
                .Select(pose => pose.position - originPosition)
                .ToArray();

            var results = vertices
                .ToArray();

            return results;
        }

        private Pose CreatePose(
            List<double> coordinates,
            Quaternion eunRotation)
        {
            var geospatialPose = geospatialMathModel.CreateGeospatialPose(
                latitude: coordinates[1],
                longitude: coordinates[0],
                altitude: coordinates[2],
                eunRotation: eunRotation);

            var pose = geospatialMathModel.CreatePose(geospatialPose);
            return pose;
        }
    }
}