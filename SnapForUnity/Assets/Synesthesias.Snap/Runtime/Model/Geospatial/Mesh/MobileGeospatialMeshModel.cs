using Cysharp.Threading.Tasks;
using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ(簡易版)のModel(携帯端末)
    /// Hullのみ対応(Holeは無視される)
    /// </summary>
    public class MobileGeospatialMeshModel : IDisposable, IGeospatialMeshModel
    {
        private readonly List<GeospatialAnchorResult> anchorResults = new();
        private readonly MeshModel meshModel;
        private readonly ITriangulationModel mobileTriangulationModel;
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly GeospatialAnchorModel geospatialAnchorModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileGeospatialMeshModel(
            MeshModel meshModel,
            ITriangulationModel mobileTriangulationModel,
            GeospatialAccuracyModel accuracyModel,
            GeospatialAnchorModel geospatialAnchorModel)
        {
            this.meshModel = meshModel;
            this.mobileTriangulationModel = mobileTriangulationModel;
            this.accuracyModel = accuracyModel;
            this.geospatialAnchorModel = geospatialAnchorModel;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            foreach (var anchorResult in anchorResults)
            {
                UnityEngine.Object.Destroy(anchorResult.Anchor.gameObject);
            }

            anchorResults.Clear();
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
            var accuracyResult = accuracyModel.GetAccuracy();

            if (!accuracyResult.IsSuccess)
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState);
            }

            if (surface?.Coordinates == null || surface.Coordinates.Count < 1)
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.EmptyCoordinate);
            }

            if (!TryGetVertexAnchors(
                    coordinates: surface.Coordinates[0], // Hullのみ対応(Holeは無視)
                    eunRotation: eunRotation,
                    results: out var vertexAnchors))
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.AnchorCreationFailed);
            }

            await UniTask.WaitUntil(
                () =>
                {
                    foreach (var vertexAnchor in vertexAnchors)
                    {
                        if (vertexAnchor.trackingState != TrackingState.Tracking)
                        {
                            return false;
                        }

                        if (!IsValidPosition(vertexAnchor.transform.position))
                        {
                            return false;
                        }
                    }

                    return true;
                },
                cancellationToken: cancellationToken);

            var originAnchor = vertexAnchors[0];
            var originVertex = originAnchor.transform.position;
            var cameraPosition = camera.transform.position - originVertex;

            var vertices = vertexAnchors
                .Select(anchor => anchor.transform.position - originVertex)
                .ToArray();

            foreach (var anchorResult in anchorResults.Skip(1)
                         .Where(anchorResult => anchorResult.Anchor)
                         .ToArray())
            {
                UnityEngine.Object.Destroy(anchorResult.Anchor.gameObject);
                anchorResults.Remove(anchorResult);
            }

            // if (!simpleMeshModel.TryCreateFanTriangles(
            //         cameraPosition: cameraPosition,
            //         vertices: vertices,
            //         results: out var triangles))
            // {
            //     return new GeospatialMeshResult(
            //         mainLoopState: accuracyResult.MainLoopState,
            //         accuracyState: accuracyResult.AccuracyState,
            //         resultType: GeospatialMeshResultType.InsufficientVertices);
            // }

            // var mesh = meshModel.CreateMesh(
            //     surface: surface,
            //     parent: originAnchor.transform,
            //     eunRotation: Quaternion.identity
            // );

            var mesh = mobileTriangulationModel.GetMesh(
                camera: camera,
                hullVertices: vertices,
                holesVertices: null);

            return new GeospatialMeshResult(
                mainLoopState: accuracyResult.MainLoopState,
                accuracyState: accuracyResult.AccuracyState,
                resultType: GeospatialMeshResultType.Success,
                anchorTransform: originAnchor.transform,
                mesh: mesh);
        }

        private bool IsValidPosition(Vector3 position)
        {
            return position != Vector3.zero &&
                   !float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z) &&
                   !float.IsInfinity(position.x) && !float.IsInfinity(position.y) && !float.IsInfinity(position.z);
        }

        private bool TryGetVertexAnchors(
            List<List<double>> coordinates,
            Quaternion eunRotation,
            out ARGeospatialAnchor[] results)
        {
            var isAnchorCreationFailed = false;

            foreach (var coordinate in coordinates)
            {
                var geospatialVector = SurfaceConverter.ToGeospatialVector(coordinate);

                var anchorResult = geospatialAnchorModel.CreateAnchor(
                    latitude: geospatialVector.Latitude,
                    longitude: geospatialVector.Longitude,
                    altitude: geospatialVector.Altitude,
                    eunRotation: eunRotation);

                anchorResults.Add(anchorResult);

                if (anchorResult.ResultType == GeospatialAnchorResultType.Success)
                {
                    continue;
                }

                isAnchorCreationFailed = true;
                break;
            }

            if (isAnchorCreationFailed)
            {
                ClearAnchors();
                results = Array.Empty<ARGeospatialAnchor>();
                return false;
            }

            results = anchorResults
                .Select(anchorResult => anchorResult.Anchor)
                .ToArray();

            return true;
        }

        private void ClearAnchors()
        {
            foreach (var anchorResult in anchorResults
                         .Where(anchorResult => anchorResult.Anchor.gameObject))
            {
                UnityEngine.Object.Destroy(anchorResult.Anchor.gameObject);
            }

            anchorResults.Clear();
        }
    }
}