using Google.XR.ARCoreExtensions;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの計算を行うModel(エディタ用)
    /// </summary>
    public class EditorGeospatialMathModel : IGeospatialMathModel
    {
        private const double MetersPerDegreeLatitude = 111111.0; // 緯度1度あたりの距離 (m)
        private const double TokyoLatitude = 36.000000; // 東京の緯度
        private const double TokyoLongitude = 139.500000; // 東京の経度
        private readonly GeospatialPose originPose;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorGeospatialMathModel()
        {
            originPose = new GeospatialPose
            {
                Latitude = TokyoLatitude,
                Longitude = TokyoLongitude,
                Altitude = 60,
                EunRotation = Quaternion.identity,
                HorizontalAccuracy = 1,
                VerticalAccuracy = 1,
                OrientationYawAccuracy = 1
            };
        }

        public GeospatialPose CreateGeospatialPose(
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation)
        {
            return new GeospatialPose
            {
                Latitude = latitude,
                Longitude = longitude,
                Altitude = altitude,
                EunRotation = eunRotation,
                HorizontalAccuracy = 1,
                VerticalAccuracy = 1,
                OrientationYawAccuracy = 1
            };
        }

        public Pose CreatePose(GeospatialPose geospatialPose)
        {
            var position = GetVector3(geospatialPose);
            var rotation = geospatialPose.EunRotation;
            return new Pose(position, rotation);
        }

        public GeospatialPose CreateGeospatialPoseAtDistance(GeospatialPose geospatialPose, float distance)
        {
            var position = GetVector3(geospatialPose);
            position += geospatialPose.EunRotation * Vector3.forward * distance;
            var result = ConvertPositionToGeospatial(position);
            return result;
        }

        public Vector2 GetVector2(GeospatialPose geospatialPose)
        {
            var x = (geospatialPose.Longitude - originPose.Longitude)
                    * MetersPerDegreeLatitude
                    * Mathf.Cos((float)(originPose.Latitude * Mathf.Deg2Rad));

            var z = (geospatialPose.Latitude - originPose.Latitude) * MetersPerDegreeLatitude;

            return new Vector2((float)x, (float)z);
        }

        public Vector3 GetVector3(GeospatialPose geospatialPose)
        {
            var x = (geospatialPose.Longitude - originPose.Longitude)
                    * MetersPerDegreeLatitude
                    * Mathf.Cos((float)(originPose.Latitude * Mathf.Deg2Rad));

            var z = (geospatialPose.Latitude - originPose.Latitude) * MetersPerDegreeLatitude;
            var y = geospatialPose.Altitude - originPose.Altitude;

            return new Vector3((float)x, (float)y, (float)z);
        }

        /// <summary>
        /// Unity のローカル座標を GeospatialPose に変換
        /// </summary>
        private GeospatialPose ConvertPositionToGeospatial(Vector3 position)
        {
            var latitude = originPose.Latitude + (position.z / MetersPerDegreeLatitude);
            var longitude = originPose.Longitude + (position.x /
                                                    (MetersPerDegreeLatitude *
                                                     Mathf.Cos((float)(originPose.Latitude * Mathf.Deg2Rad))));

            var altitude = originPose.Altitude + position.y;

            var result = CreateGeospatialPose(
                latitude: latitude,
                longitude: longitude,
                altitude: altitude,
                eunRotation: Quaternion.identity);

            return result;
        }
    }
}