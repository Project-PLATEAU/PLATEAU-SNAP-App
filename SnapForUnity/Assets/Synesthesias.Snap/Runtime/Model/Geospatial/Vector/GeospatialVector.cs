namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialのベクトル
    /// </summary>
    public struct GeospatialVector
    {
        /// <summary>
        /// 緯度
        /// </summary>
        public readonly double Latitude;

        /// <summary>
        /// 経度
        /// </summary>
        public readonly double Longitude;

        /// <summary>
        /// 高度
        /// </summary>
        public readonly double Altitude;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialVector(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }
    }
}