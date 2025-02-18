using System;

namespace Synesthesias.PLATEAU.Snap.Generated.Model
{
    /// <summary>
    /// 建物画像のメタデータ
    /// spec.ymlに含まれていないためopenapi-generatorでは生成されないモデルを定義
    /// </summary>
    [Serializable]
    public class BuildingImageMetadata
    {
        public string gmlid;
        public Coordinate from;
        public Coordinate to;
        public double roll;
        public DateTime timestamp;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gmlid"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="roll"></param>
        /// <param name="timestamp"></param>
        public BuildingImageMetadata(
            string gmlid,
            Coordinate from,
            Coordinate to,
            double roll,
            DateTime timestamp)
        {
            this.gmlid = gmlid;
            this.from = from;
            this.to = to;
            this.roll = roll;
            this.timestamp = timestamp;
        }

        /// <summary>
        /// JSONに変換
        /// </summary>
        public string ToJson()
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            return result;
        }
    }
}