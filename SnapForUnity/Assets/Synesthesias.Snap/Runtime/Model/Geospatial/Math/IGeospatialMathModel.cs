using Google.XR.ARCoreExtensions;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの計算を行うModel
    /// </summary>
    public interface IGeospatialMathModel
    {
        /// <summary>
        /// GeospatialPoseを作成
        /// </summary>
        GeospatialPose CreateGeospatialPose(
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation);

        /// <summary>
        /// GeospatialPoseからPoseを作成する
        /// </summary>
        Pose CreatePose(GeospatialPose geospatialPose);

        /// <summary>
        /// 指定した位置の座標(Vector2)を取得する
        /// </summary>
        Vector2 GetVector2(GeospatialPose geospatialPose);

        /// <summary>
        /// 指定した位置の座標(Vector3)を取得する
        /// </summary>
        Vector3 GetVector3(GeospatialPose geospatialPose);

        /// <summary>
        /// 指定した位置から指定した距離だけ離れた位置のGeospatialPoseを作成する
        /// </summary>
        GeospatialPose CreateGeospatialPoseAtDistance(
            GeospatialPose geospatialPose,
            float distance);
    }
}