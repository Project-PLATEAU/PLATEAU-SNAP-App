using Google.XR.ARCoreExtensions;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// Geospatialの計算を行うModel
    /// </summary>
    public class GeospatialMathModel
    {
        private readonly AREarthManager arEarthManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialMathModel(AREarthManager arEarthManager)
        {
            this.arEarthManager = arEarthManager;
        }

        /// <summary>
        /// 指定した位置から指定した距離だけ離れた位置のGeospatialPoseを作成する
        /// </summary>
        public GeospatialPose CreateGeospatialPoseAtDistance(
            GeospatialPose geospatialPose,
            float distance)
        {
            var pose = arEarthManager.Convert(geospatialPose);
            pose.position += pose.rotation * Vector3.forward * distance;
            var result = arEarthManager.Convert(pose);
            return result;
        }
    }
}