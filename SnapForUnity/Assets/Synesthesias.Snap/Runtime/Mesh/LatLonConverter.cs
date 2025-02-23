using System;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    public static class LatLonConverter
    {
        // 地球の半径 (メートル)
        private const double EarthRadius = 6378137.0;

        // Playerの緯度経度
        private const double PlayerLat = 35.681301;
        private const double PlayerLon = 139.771352;
        private const double PlayerAltitude = 39.8;

        // 平面直角座標系の原点の緯度経度
        private const double TokyoLat = 36.000000;
        private const double TokyoLon = 139.500000;

        /// <summary>
        /// 緯度経度高度をメートルに変換
        /// </summary>
        /// <param name="longitude">頂点の経度</param>
        /// <param name="latitude">頂点の緯度</param>
        /// <param name="altitude">頂点の高度</param>
        /// <param name="originLongitude">メッシュ原点の経度</param>
        /// <param name="originLatitude">メッシュ原点の緯度</param>
        /// <param name="originAltitude">メッシュ原点の高度</param>
        /// <returns>(経度, 高度, 緯度)</returns>
        public static Vector3 ToMeters(double longitude, double latitude, double altitude, double originLongitude, double originLatitude, double originAltitude)
        {
            // 頂点のX座標とZ座標（平面直角座標）
            var (vX, vZ) = CoordinateUtil.JGD2011ToPlaneRectCoord(latitude, longitude, TokyoLat, TokyoLon);
            // メッシュ原点(メッシュの中心)のX座標とZ座標（平面直角座標）
            var (oX, oZ) = CoordinateUtil.JGD2011ToPlaneRectCoord(originLatitude, originLongitude, TokyoLat, TokyoLon);

            // 差分
            double deltaX = vX - oX;
            double deltaY = altitude - originAltitude;
            double deltaZ = vZ - oZ;

            // 平面直角座標では、Xが南北方向、Zが東西方向を表すので、XとZを入れ替えている
            return new Vector3((float)deltaZ, (float)deltaY, (float)deltaX);
        }
    }
}