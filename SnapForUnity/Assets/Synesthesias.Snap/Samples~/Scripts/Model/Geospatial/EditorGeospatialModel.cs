using Google.XR.ARCoreExtensions;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// Geospatialの計算を行うModel(エディタ)
    /// </summary>
    public class EditorGeospatialModel
    {
        /// <summary>
        /// GeospatialPoseを作成する
        /// </summary>
        public GeospatialPose CreateGeospatialPose(
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation)
        {
            var result = new GeospatialPose
            {
                Latitude = latitude, Longitude = longitude, Altitude = altitude, EunRotation = eunRotation
            };

            return result;
        }
    }
}