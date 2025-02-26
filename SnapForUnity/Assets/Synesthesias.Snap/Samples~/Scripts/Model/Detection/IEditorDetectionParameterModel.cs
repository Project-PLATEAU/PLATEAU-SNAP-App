using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のパラメータのModel
    /// </summary>
    public interface IEditorDetectionParameterModel
    {
        /// <summary>
        /// 始点の緯度
        /// </summary>
        public double FromLatitude { get; }

        /// <summary>
        /// 始点の経度
        /// </summary>
        public double FromLongitude { get; }

        /// <summary>
        /// 始点の高度
        /// </summary>
        public double FromAltitude { get; }

        /// <summary>
        /// 最大距離
        /// </summary>
        public double MaxDistance { get; }

        /// <summary>
        /// 視野角
        /// </summary>
        public double FieldOfView { get; }

        /// <summary>
        /// カメラの角度
        /// </summary>
        public Quaternion EunRotation { get; }
    }
}