using Cysharp.Threading.Tasks;
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
        private readonly IMeshFactoryModel meshFactoryModel;
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly GeospatialAnchorModel geospatialAnchorModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileGeospatialMeshModel(
            IMeshFactoryModel meshFactoryModel,
            GeospatialAccuracyModel accuracyModel,
            GeospatialAnchorModel geospatialAnchorModel)
        {
            this.meshFactoryModel = meshFactoryModel;
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
                    results: out var results))
            {
                ClearAnchors(results);

                return new GeospatialMeshResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialMeshResultType.AnchorCreationFailed);
            }

            anchorResults.AddRange(results);

            await UniTask.WaitUntil(
                () =>
                {
                    foreach (var vertexAnchor in results
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

            var originAnchor = results[0].Anchor;
            var originVertex = originAnchor.transform.position;

            var vertices = results
                .Select(result => result.Anchor.transform.position - originVertex)
                .ToArray();

            // Verticesを取得したので原点以外のアンカーを削除
            for (var anchorIndex = 1; anchorIndex < results.Count; anchorIndex++)
            {
                var anchorResult = results[anchorIndex];
                if (anchorResult.Anchor.gameObject)
                {
                    UnityEngine.Object.Destroy(anchorResult.Anchor.gameObject);
                }

                results.Remove(anchorResult);
                anchorResults.Remove(anchorResult);
            }

            var mesh = await meshFactoryModel.CreateAsync(
                hull: vertices,
                holes: null,
                cancellationToken: cancellationToken);

            var result = new GeospatialMeshResult(
                mainLoopState: accuracyResult.MainLoopState,
                accuracyState: accuracyResult.AccuracyState,
                resultType: GeospatialMeshResultType.Success,
                anchorTransform: originAnchor.transform,
                mesh: mesh);

            return result;
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

                if (anchorResult.IsSuccess)
                {
                    continue;
                }

                return false;
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