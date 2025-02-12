using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    public static class LatLonConverter
    {
        // 地球の半径 (メートル)
        private const double EarthRadius = 6378137.0;

        // Playerの緯度経度
        private const double PlayerLat = 139.7792;
        private const double PlayerLon = 35.6845;
        private const double PlayerAltitude = 39.8;

        // 平面直角座標系の原点の緯度経度
        private const double TokyoLat = 139.500000;
        private const double TokyoLon = 36.000000;

        public static Vector3 ToMeters(double latitude, double longitude, double altitude)
        {
            // 頂点のX座標とZ座標（平面直角座標）
            var (vX, vZ) = CoordinateUtil.JGD2011ToPlaneRectCoord(latitude, longitude, TokyoLat, TokyoLon);
            // プレイヤーのX座標とZ座標（平面直角座標）
            var (pX, pZ) = CoordinateUtil.JGD2011ToPlaneRectCoord(PlayerLat, PlayerLon, TokyoLat, TokyoLon);

            // 差分
            double deltaX = vX - pX;
            double deltaY = altitude - PlayerAltitude;
            double deltaZ = vZ - pZ;

            return new Vector3((float)deltaX, (float)deltaY, (float)deltaZ);
        }
    }
}