using Google.XR.ARCoreExtensions;

namespace Synesthesias.Snap.Runtime
{
    public class GeospatialPoseModel
    {
        private readonly AREarthManager arEarthManager;

        public GeospatialPoseModel(AREarthManager arEarthManager)
        {
            this.arEarthManager = arEarthManager;
        }

        /// <summary>
        /// カメラのPoseを取得する
        /// </summary>
        public GeospatialPose GetCameraPose()
        {
            return arEarthManager.CameraGeospatialPose;
        }
    }
}