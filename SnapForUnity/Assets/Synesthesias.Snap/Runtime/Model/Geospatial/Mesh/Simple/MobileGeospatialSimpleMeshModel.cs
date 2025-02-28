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
    public class MobileGeospatialSimpleMeshModel : IDisposable, IGeospatialSimpleMeshModel
    {
        private readonly List<GeospatialAnchorResult> anchorResults = new();
        private readonly SimpleMeshModel simpleMeshModel;
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly GeospatialAnchorModel geospatialAnchorModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileGeospatialSimpleMeshModel(
            SimpleMeshModel simpleMeshModel,
            GeospatialAccuracyModel accuracyModel,
            GeospatialAnchorModel geospatialAnchorModel)
        {
            this.simpleMeshModel = simpleMeshModel;
            this.accuracyModel = accuracyModel;
            this.geospatialAnchorModel = geospatialAnchorModel;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            ClearAnchors(anchorResults);
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

            var coordinates = surface?.GetUniqueCoordinates();

            if (coordinates == null || coordinates.Count < 1)
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.EmptyCoordinate);
            }

            if (!TryGetAnchorResults(
                    coordinates: coordinates[0], // Hullのみ対応(Holeは無視)
                    eunRotation: eunRotation,
                    results: out var anchorResults))
            {
                ClearAnchors(anchorResults);

                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.AnchorCreationFailed);
            }

            this.anchorResults.AddRange(anchorResults);

            await UniTask.WaitUntil(
                () =>
                {
                    foreach (var vertexAnchor in anchorResults
                                 .Select(result => result.Anchor))
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

            var originAnchor = anchorResults[0].Anchor;
            var originVertex = originAnchor.transform.position;
            var cameraPosition = camera.transform.position - originVertex;

            var vertices = anchorResults
                .Select(result => result.Anchor.transform.position - originVertex)
                .ToArray();

            ClearAnchors(anchorResults);

            if (!simpleMeshModel.TryCreateFanTriangles(
                    cameraPosition: cameraPosition,
                    vertices: vertices,
                    results: out var triangles))
            {
                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.InsufficientVertices);
            }

            var mesh = simpleMeshModel.CreateMesh(vertices, triangles);

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

        private bool TryGetAnchorResults(
            List<List<double>> coordinates,
            Quaternion eunRotation,
            out List<GeospatialAnchorResult> results)
        {
            results = new List<GeospatialAnchorResult>();

            foreach (var coordinate in coordinates)
            {
                var geospatialVector = SurfaceConverter.ToGeospatialVector(coordinate);

                var anchorResult = geospatialAnchorModel.CreateAnchor(
                    latitude: geospatialVector.Latitude,
                    longitude: geospatialVector.Longitude,
                    altitude: geospatialVector.Altitude,
                    eunRotation: eunRotation);

                results.Add(anchorResult);

                if (!anchorResult.IsSuccess)
                {
                    return false;
                }
            }

            return true;
        }

        private static void ClearAnchors(List<GeospatialAnchorResult> anchorResults)
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