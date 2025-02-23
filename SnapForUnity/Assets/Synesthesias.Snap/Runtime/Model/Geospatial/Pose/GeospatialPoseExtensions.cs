using Google.XR.ARCoreExtensions;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatial Poseの拡張メソッド
    /// </summary>
    public static class GeospatialPoseExtensions
    {
        /// <summary>
        /// Poseが有効かどうか
        /// </summary>
        public static bool IsValid(this GeospatialPose pose)
        {
            var result = pose.Longitude != 0 && pose.Latitude != 0;
            return result;
        }
    }
}