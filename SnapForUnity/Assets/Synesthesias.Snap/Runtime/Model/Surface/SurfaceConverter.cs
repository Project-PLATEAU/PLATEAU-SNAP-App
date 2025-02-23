using System.Collections.Generic;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// 面の変換
    /// </summary>
    public static class SurfaceConverter
    {
        /// <summary>
        /// GeospatialVectorに変換
        /// </summary>
        public static GeospatialVector ToGeospatialVector(IReadOnlyList<double> coordinate)
        {
            return new GeospatialVector(
                latitude: coordinate[1],
                longitude: coordinate[0],
                altitude: coordinate[2]);
        }
    }
}