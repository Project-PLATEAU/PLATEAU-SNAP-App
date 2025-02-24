using Google.XR.ARCoreExtensions;
using Synesthesias.PLATEAU.Snap.Generated.Model;
using Synesthesias.Snap.Runtime;
using System;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 検証するパラメータのModel
    /// </summary>
    public class ValidationParameterModel
    {
        /// <summary>
        /// Meshの検証結果
        /// </summary>
        public readonly MeshValidationResult MeshValidationResult;

        /// <summary>
        /// GML ID
        /// </summary>
        public readonly string GmlId;

        /// <summary>
        /// 開始地点の座標
        /// </summary>
        public readonly Coordinate FromCoordinate;

        /// <summary>
        /// 終了地点の座標
        /// </summary>
        public readonly Coordinate ToCoordinate;

        /// <summary>
        /// ロール
        /// </summary>
        public readonly double Roll;

        /// <summary>
        /// タイムスタンプ
        /// </summary>
        public readonly DateTime Timestamp;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationParameterModel(
            MeshValidationResult meshValidationResult,
            string gmlId,
            Coordinate fromCoordinate,
            Coordinate toCoordinate,
            double roll,
            DateTime timestamp)
        {
            MeshValidationResult = meshValidationResult;
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
            MeshValidationResult meshValidationResult,
            string gmlId,
            double fromLongitude,
            double fromLatitude,
            double fromAltitude,
            double toLongitude,
            double toLatitude,
            double toAltitude,
            double roll,
            DateTime timestamp) : this(
            meshValidationResult: meshValidationResult,
            gmlId: gmlId,
            fromCoordinate: new Coordinate(fromLongitude, fromLatitude, fromAltitude),
            toCoordinate: new Coordinate(toLongitude, toLatitude, toAltitude),
            roll: roll,
            timestamp: timestamp)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationParameterModel(
            MeshValidationResult meshValidationResult,
            string gmlId,
            GeospatialPose fromGeospatialPose,
            GeospatialPose toGeospatialPose,
            double roll,
            DateTime timestamp) : this(
            meshValidationResult: meshValidationResult,
            gmlId: gmlId,
            fromLongitude: fromGeospatialPose.Longitude,
            fromLatitude: fromGeospatialPose.Latitude,
            fromAltitude: fromGeospatialPose.Altitude,
            toLongitude: toGeospatialPose.Longitude,
            toLatitude: toGeospatialPose.Latitude,
            toAltitude: toGeospatialPose.Altitude,
            roll: roll,
            timestamp: timestamp)
        {
        }
    }
}