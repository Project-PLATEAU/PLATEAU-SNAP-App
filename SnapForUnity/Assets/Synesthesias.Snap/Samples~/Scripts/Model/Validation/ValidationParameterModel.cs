using Google.XR.ARCoreExtensions;
using Synesthesias.PLATEAU.Snap.Generated.Model;
using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 検証するパラメータのModel
    /// </summary>
    public class ValidationParameterModel
    {
        public Vector3 CameraPosition { get; private set; }
        public Mesh Mesh { get; private set; }
        public Transform MeshTransform { get; private set; }
        public string GmlId { get; private set; }
        public Coordinate FromCoordinate { get; private set; }
        public Coordinate ToCoordinate { get; private set; }
        public double Roll { get; private set; }
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationParameterModel(
            Vector3 cameraPosition,
            Mesh mesh,
            Transform meshTransform,
            string gmlId,
            Coordinate fromCoordinate,
            Coordinate toCoordinate,
            double roll,
            DateTime timestamp)
        {
            CameraPosition = cameraPosition;
            Mesh = mesh;
            MeshTransform = meshTransform;
            GmlId = gmlId;
            FromCoordinate = fromCoordinate;
            ToCoordinate = toCoordinate;
            Roll = roll;
            Timestamp = timestamp;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationParameterModel(
            Vector3 cameraPosition,
            Mesh mesh,
            Transform meshTransform,
            string gmlId,
            double fromLongitude,
            double fromLatitude,
            double fromAltitude,
            double toLongitude,
            double toLatitude,
            double toAltitude,
            double roll,
            DateTime timestamp) : this(
            cameraPosition,
            mesh,
            meshTransform,
            gmlId,
            fromCoordinate: new Coordinate(fromLongitude, fromLatitude, fromAltitude),
            toCoordinate: new Coordinate(toLongitude, toLatitude, toAltitude),
            roll,
            timestamp)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationParameterModel(
            Vector3 cameraPosition,
            Mesh mesh,
            Transform meshTransform,
            string gmlId,
            GeospatialPose fromGeospatialPose,
            GeospatialPose toGeospatialPose,
            double roll,
            DateTime timestamp) : this(
            cameraPosition,
            mesh,
            meshTransform,
            gmlId,
            fromLongitude: fromGeospatialPose.Longitude,
            fromLatitude: fromGeospatialPose.Latitude,
            fromAltitude: fromGeospatialPose.Altitude,
            toLongitude: toGeospatialPose.Longitude,
            toLatitude: toGeospatialPose.Latitude,
            toAltitude: toGeospatialPose.Altitude,
            roll,
            timestamp)
        {
        }
    }
}