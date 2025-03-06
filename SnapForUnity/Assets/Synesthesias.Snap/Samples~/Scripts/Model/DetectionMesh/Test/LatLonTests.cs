using System.Collections.Generic;

namespace Synesthesias.Snap.Sample
{
    public static class LatLonTests
    {
        public struct LatLonData
        {
            public double[][] hull;
            public List<double[][]> holes;
        }

        public static LatLonData[] TestData = new LatLonData[5];
    }
}