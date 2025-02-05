using Cysharp.Threading.Tasks;
using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// Geospatial関連のModel(非同期処理)
    /// GeospatialControllerを参考にUniTaskを使用して非同期処理を行なっている
    /// </summary>
    public class GeospatialAsyncModel
    {
        private readonly ARAnchorManager arAnchorManager;
        private readonly AREarthManager arEarthManager;
        private readonly ARRaycastManager arRaycastManager;
        private readonly ARStreetscapeGeometryManager arStreetScapeGeometryManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAsyncModel(
            ARAnchorManager arAnchorManager,
            AREarthManager arEarthManager,
            ARRaycastManager arRaycastManager,
            ARStreetscapeGeometryManager arStreetScapeGeometryManager)
        {
            this.arAnchorManager = arAnchorManager;
            this.arEarthManager = arEarthManager;
            this.arRaycastManager = arRaycastManager;
            this.arStreetScapeGeometryManager = arStreetScapeGeometryManager;
        }

        /// <summary>
        /// カメラのGeospatialベースのポーズを取得する
        /// </summary>
        public GeospatialPose GetCameraGeospatialPose()
        {
            var result = arEarthManager.CameraGeospatialPose;
            return result;
        }

        /// <summary>
        /// ARGeospatialアンカーを作成する(AnchorType指定)
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorWithAnchorTypeAsync(
            AnchorType anchorType,
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            switch (anchorType)
            {
                case AnchorType.Rooftop:
                    var roofTopResult = await CreateARGeospatialAnchorAsRooftopAsync(
                        latitude: latitude,
                        longitude: longitude,
                        altitudeAboveRooftop: altitude,
                        eunRotation: eunRotation,
                        cancellationToken: cancellationToken);

                    return roofTopResult;
                case AnchorType.Terrain:
                    var terrainResult = await CreateARGeospatialAnchorAsTerrainAsync(
                        latitude: latitude,
                        longitude: longitude,
                        altitudeAboveTerrain: altitude,
                        eunRotation: eunRotation,
                        cancellationToken: cancellationToken);

                    return terrainResult;
                case AnchorType.Geospatial:
                default:
                    var defaultResult = await CreateARGeospatialAnchorAsync(
                        latitude: latitude,
                        longitude: longitude,
                        altitude: altitude,
                        eunRotation: eunRotation,
                        cancellationToken: cancellationToken);

                    return defaultResult;
            }
        }

        /// <summary>
        /// ARGeospatialアンカーを作成する(AnchorType指定)
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorWithAnchorTypeAsync(
            AnchorType anchorType,
            GeospatialPose geospatialPose,
            CancellationToken cancellationToken)
        {
            var result = anchorType switch
            {
                AnchorType.Rooftop => await CreateARGeospatialAnchorAsRooftopAsync(
                    geospatialPose,
                    cancellationToken),
                AnchorType.Terrain => await CreateARGeospatialAnchorAsTerrainAsync(
                    geospatialPose,
                    cancellationToken),
                _ => await CreateARGeospatialAnchorAsync(
                    geospatialPose,
                    cancellationToken)
            };

            return result;
        }

        /// <summary>
        /// ARGeospatialアンカー(RoofTop)を作成する
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorAsRooftopAsync(
            double latitude,
            double longitude,
            double altitudeAboveRooftop,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var promise = arAnchorManager.ResolveAnchorOnRooftopAsync(
                latitude: latitude,
                longitude: longitude,
                altitudeAboveRooftop: altitudeAboveRooftop,
                eunRotation: eunRotation);

            await promise.WithCancellation(cancellationToken);

            if (promise.Result.RooftopAnchorState != RooftopAnchorState.Success)
            {
                throw new OperationCanceledException(
                    "Failed to create Geospatial Rooftop Anchor." +
                    "Reason: " + promise.Result.RooftopAnchorState);
            }

            var result = promise.Result.Anchor;
            return result;
        }

        /// <summary>
        /// ARGeospatialアンカー(RoofTop)を作成する
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorAsRooftopAsync(
            GeospatialPose geospatialPose,
            CancellationToken cancellationToken)
        {
            var result = await CreateARGeospatialAnchorAsRooftopAsync(
                latitude: geospatialPose.Latitude,
                longitude: geospatialPose.Longitude,
                altitudeAboveRooftop: geospatialPose.Altitude,
                eunRotation: geospatialPose.EunRotation,
                cancellationToken: cancellationToken);

            return result;
        }

        /// <summary>
        /// ARGeospatialアンカー(Terrain)を作成する
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorAsTerrainAsync(
            double latitude,
            double longitude,
            double altitudeAboveTerrain,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var promise = arAnchorManager.ResolveAnchorOnTerrainAsync(
                latitude: latitude,
                longitude: longitude,
                altitudeAboveTerrain: altitudeAboveTerrain,
                eunRotation: eunRotation);

            await promise.WithCancellation(cancellationToken);

            if (promise.Result.TerrainAnchorState != TerrainAnchorState.Success)
            {
                throw new OperationCanceledException(
                    "Failed to create Geospatial Rooftop Anchor." +
                    "Reason: " + promise.Result.TerrainAnchorState);
            }

            var result = promise.Result.Anchor;
            return result;
        }

        /// <summary>
        /// ARGeospatialアンカー(Terrain)を作成する
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorAsTerrainAsync(
            GeospatialPose geospatialPose,
            CancellationToken cancellationToken)
        {
            var result = await CreateARGeospatialAnchorAsTerrainAsync(
                latitude: geospatialPose.Latitude,
                longitude: geospatialPose.Longitude,
                altitudeAboveTerrain: geospatialPose.Altitude,
                eunRotation: geospatialPose.EunRotation,
                cancellationToken: cancellationToken);

            return result;
        }

        /// <summary>
        /// ARGeospatialアンカーを作成する
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorAsync(
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var anchor = arAnchorManager.AddAnchor(
                latitude: latitude,
                longitude: longitude,
                altitude: altitude,
                eunRotation: eunRotation);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            return anchor;
        }

        /// <summary>
        /// ARGeospatialアンカーを作成する
        /// </summary>
        public async UniTask<ARGeospatialAnchor> CreateARGeospatialAnchorAsync(
            GeospatialPose geospatialPose,
            CancellationToken cancellationToken)
        {
            var anchor = await CreateARGeospatialAnchorAsync(
                latitude: geospatialPose.Latitude,
                longitude: geospatialPose.Longitude,
                altitude: geospatialPose.Altitude,
                eunRotation: geospatialPose.EunRotation,
                cancellationToken: cancellationToken);

            return anchor;
        }

        /// <summary>
        /// StreetScapeのARアンカーを作成する
        /// </summary>
        public async UniTask<ARAnchor> CreateARAnchorAsStreetScapeAsync(
            Pose pose,
            TrackableId trackableId,
            CancellationToken cancellationToken)
        {
            var geometry = arStreetScapeGeometryManager.GetStreetscapeGeometry(trackableId);

            if (geometry == null)
            {
                return null;
            }

            var anchor = arStreetScapeGeometryManager.AttachAnchor(
                geometry: geometry,
                pose: pose);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            return anchor;
        }

        /// <summary>
        /// RaycastHitの位置のGeospatial+StreetScapeのARアンカーを作成する
        /// </summary>
        public async UniTask<ARAnchor> CreateARAnchorAsStreetScapeAsync(
            XRRaycastHit raycastHit,
            CancellationToken cancellationToken)
        {
            var anchor = await CreateARAnchorAsStreetScapeAsync(
                pose: raycastHit.pose,
                trackableId: raycastHit.trackableId,
                cancellationToken: cancellationToken);

            return anchor;
        }

        /// <summary>
        /// スクリーン上の位置にStreetScapeアンカーを作成する
        /// </summary>
        public async UniTask<ARAnchor> CreateARAnchorAsStreetScapeAsync(
            AnchorType anchorType,
            XRRaycastHit hitResult,
            CancellationToken cancellationToken)
        {
            var isRoofTopOrTerrain = anchorType is AnchorType.Rooftop or AnchorType.Terrain;

            var result = isRoofTopOrTerrain
                ? await CreateARAnchorAsRootTopOrTerrainStreetScapeAsync(
                    hitResult: hitResult,
                    cancellationToken: cancellationToken)
                : await CreateARAnchorAsStreetScapeAsync(
                    raycastHit: hitResult,
                    cancellationToken: cancellationToken);

            return result;
        }

        /// <summary>
        /// AR空間にRaycastを行いRaycastHitを取得する
        /// </summary>
        public bool ARRaycast(
            Vector2 screenPosition,
            ref List<XRRaycastHit> results)
        {
            var result = arRaycastManager.RaycastStreetscapeGeometry(screenPosition, ref results);
            return result;
        }

        /// <summary>
        /// RaycastHitの位置のRoofTop/TerrainのStreetScapeアンカーを作成する
        /// </summary>
        private async UniTask<ARAnchor> CreateARAnchorAsRootTopOrTerrainStreetScapeAsync(
            XRRaycastHit hitResult,
            CancellationToken cancellationToken)
        {
            var pose = new Pose(
                position: hitResult.pose.position,
                rotation: Quaternion.LookRotation(Vector3.right, Vector3.up));

            var anchor = await CreateARAnchorAsStreetScapeAsync(
                pose: pose,
                trackableId: hitResult.trackableId,
                cancellationToken: cancellationToken);

            return anchor;
        }
    }
}