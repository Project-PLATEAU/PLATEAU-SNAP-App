using Cysharp.Threading.Tasks;
using Google.XR.ARCoreExtensions;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using Synesthesias.PLATEAU.Snap.Generated.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 現在の位置で撮影可能な面の情報を取得するリポジトリ
    /// </summary>
    public class SurfaceRepository
    {
        private readonly TimeSpan cacheDuration;
        private readonly ISurfacesApiAsync surfacesApiAsync;
        private DateTime requestedAt;
        private IReadOnlyList<Surface> cachedSurfaces;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SurfaceRepository(ISurfacesApiAsync surfacesApiAsync)
        {
            cacheDuration = TimeSpan.FromSeconds(1);
            this.surfacesApiAsync = surfacesApiAsync;
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得
        /// </summary>
        /// <param name="fromLatitude">始点の緯度</param>
        /// <param name="fromLongitude">始点の経度</param>
        /// <param name="fromAltitude">始点の高度</param>
        /// <param name="toLatitude">終点の緯度</param>
        /// <param name="toLongitude">終点の経度</param>
        /// <param name="toAltitude">終点の高度</param>
        /// <param name="roll">回転</param>
        /// <param name="maxDistance">最大距離</param>
        /// <param name="fieldOfView">視野角</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>面の配列</returns>
        public async UniTask<IReadOnlyList<Surface>> GetVisibleSurfacesAsync(
            double fromLatitude,
            double fromLongitude,
            double fromAltitude,
            double toLatitude,
            double toLongitude,
            double toAltitude,
            double roll,
            double maxDistance,
            double fieldOfView,
            CancellationToken cancellationToken)
        {
            if (TryGetCachedSurfaces(out var result))
            {
                return result;
            }

            var previousRequestedAt = requestedAt;

            try
            {
                var fromCoordinate = new Coordinate(
                    latitude: fromLatitude,
                    longitude: fromLongitude,
                    altitude: fromAltitude);

                var toCoordinate = new Coordinate(
                    latitude: toLatitude,
                    longitude: toLongitude,
                    altitude: toAltitude);

                var request = new VisibleSurfacesRequest(
                    from: fromCoordinate,
                    to: toCoordinate,
                    roll: roll,
                    maxDistance: maxDistance,
                    fieldOfView: fieldOfView);

                requestedAt = DateTime.UtcNow;

                var response = await surfacesApiAsync.GetVisibleSurfacesAsyncAsync(
                    visibleSurfacesRequest: request,
                    cancellationToken: cancellationToken);

                cachedSurfaces = response.Surfaces;

                result = cachedSurfaces;
                return result;
            }
            catch (ApiException exception)
            {
                Debug.LogError(exception);
                requestedAt = previousRequestedAt;
                throw;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                requestedAt = previousRequestedAt;
                throw;
            }
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得
        /// </summary>
        /// <param name="fromGeospatialPose">始点のGeospatial情報</param>
        /// <param name="toGeospatialPose">終点のGeospatial情報</param>
        /// <param name="roll">回転</param>
        /// <param name="maxDistance">最大距離</param>
        /// <param name="fieldOfView">視野角</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>面の配列</returns>
        public async UniTask<IReadOnlyList<Surface>> GetVisibleSurfacesAsync(
            GeospatialPose fromGeospatialPose,
            GeospatialPose toGeospatialPose,
            double roll,
            double maxDistance,
            double fieldOfView,
            CancellationToken cancellationToken)
        {
            var result = await GetVisibleSurfacesAsync(
                fromLatitude: fromGeospatialPose.Latitude,
                fromLongitude: fromGeospatialPose.Longitude,
                fromAltitude: fromGeospatialPose.Altitude,
                toLatitude: toGeospatialPose.Latitude,
                toLongitude: toGeospatialPose.Longitude,
                toAltitude: toGeospatialPose.Altitude,
                roll: roll,
                maxDistance: maxDistance,
                fieldOfView: fieldOfView,
                cancellationToken: cancellationToken);

            return result;
        }

        /// <summary>
        /// 現在の位置で撮影可能な面の情報を取得
        /// </summary>
        /// <param name="fromGeospatialPose">始点のGeospatial情報</param>
        /// <param name="toGeospatialPose">終点のGeospatial情報</param>
        /// <param name="camera">カメラ</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>面の配列</returns>
        public async UniTask<IReadOnlyList<Surface>> GetVisibleSurfacesAsync(
            GeospatialPose fromGeospatialPose,
            GeospatialPose toGeospatialPose,
            Camera camera,
            CancellationToken cancellationToken)
        {
            var result = await GetVisibleSurfacesAsync(
                fromLatitude: fromGeospatialPose.Latitude,
                fromLongitude: fromGeospatialPose.Longitude,
                fromAltitude: fromGeospatialPose.Altitude,
                toLatitude: toGeospatialPose.Latitude,
                toLongitude: toGeospatialPose.Longitude,
                toAltitude: toGeospatialPose.Altitude,
                roll: camera.transform.rotation.eulerAngles.z,
                maxDistance: camera.farClipPlane,
                fieldOfView: camera.fieldOfView,
                cancellationToken: cancellationToken);

            return result;
        }

        private bool TryGetCachedSurfaces(out IReadOnlyList<Surface> result)
        {
            if (cachedSurfaces == null)
            {
                result = null;
                return false;
            }

            if (DateTime.UtcNow - requestedAt > cacheDuration)
            {
                result = null;
                return false;
            }

            result = cachedSurfaces;
            return true;
        }
    }
}