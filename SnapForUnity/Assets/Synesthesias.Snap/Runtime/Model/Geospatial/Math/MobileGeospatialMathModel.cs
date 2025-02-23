using Google.XR.ARCoreExtensions;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの計算を行うModel(携帯端末)
    /// </summary>
    public class MobileGeospatialMathModel : IGeospatialMathModel
    {
        private readonly AREarthManager arEarthManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileGeospatialMathModel(AREarthManager arEarthManager)
        {
            this.arEarthManager = arEarthManager;
        }

        /// <summary>
        /// GeospatialPoseを作成
        /// </summary>
        public GeospatialPose CreateGeospatialPose(
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation)
        {
            var result = new GeospatialPose
            {
                Latitude = latitude,
                Longitude = longitude,
                Altitude = altitude,
                EunRotation = eunRotation,
                HorizontalAccuracy = 1,
                VerticalAccuracy = 1
            };

            return result;
        }

        /// <summary>
        /// GeospatialPoseからPoseを作成する
        /// </summary>
        public Pose CreatePose(GeospatialPose geospatialPose)
        {
            var result = arEarthManager.Convert(geospatialPose);
            return result;
        }

        /// <summary>
        /// 指定した位置から指定した距離だけ離れた位置のGeospatialPoseを作成する
        /// </summary>
        public GeospatialPose CreateGeospatialPoseAtDistance(
            GeospatialPose geospatialPose,
            float distance)
        {
            var pose = CreatePose(geospatialPose);
            pose.position += pose.rotation * Vector3.forward * distance;
            var result = arEarthManager.Convert(pose);
            return result;
        }

        public Vector3 GetVector3(GeospatialPose geospatialPose)
        {
            var pose = arEarthManager.Convert(geospatialPose);
            var result = pose.position;
            return result;
        }

        public Vector2 GetVector2(GeospatialPose geospatialPose)
        {
            var pose = arEarthManager.Convert(geospatialPose);
            var result = pose.position;
            return new Vector2(result.x, result.z);
        }
    }
}