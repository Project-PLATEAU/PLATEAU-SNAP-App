using System.Collections.Generic;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// 面の情報
    /// </summary>
    public interface ISurfaceModel
    {
        /// <summary>
        /// GML ID
        /// </summary>
        public string GmlId { get; set; }

        /// <summary>
        /// 面の座標
        /// </summary>
        /// <remarks>
        /// 内側から数えて
        /// [1番目のList] - 座標の配列
        /// 0インデックス目: 緯度(Latitude)の座標
        /// 1インデックス目: 経度(Longitude)の座標
        /// 2インデックス目: 高度(Altitude)の座標
        /// 
        /// [2番目のList] - 面と穴のデータ
        /// 0インデックス目: 面(Hull)の座標の配列
        /// 1インデックス目以降: 穴(Hole)の座標の配列(面がくり抜かれているいる場合)
        /// 
        /// [3番目のList] - 面と穴のデータの配列
        /// </remarks>
        public List<List<List<double>>> Coordinates { get; set; }
    }
}