using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Synesthesias.Snap.Runtime
{
    // JSON全体のモデル(モック版)
    public class MockSurfaces
    {
        [JsonProperty("surfaces")]
        public IList<ISurfaceModel> DetectedSurfaces { get; set; } = new List<ISurfaceModel>();
    }

    // 各surfaceのモデル(モック版)
    public class MockSurface : ISurfaceModel
    {
        [JsonProperty("gmlid")]
        public string GmlId { get; set; }

        [JsonProperty("coordinates")]
        public List<List<List<double>>> Coordinates { get; set; }

        public List<List<List<double>>> GetUniqueCoordinates()
            => Coordinates
                .SkipLast(1)
                .ToList();
    }
    public class MockJsonParser
    {
        public static MockSurfaces Parse()
        {
            string json = @"{
  ""surfaces"": [
    {
      ""gmlid"": ""ID_e9b501d2-2aaa-4f56-a738-42ddb8aea87a"",
      ""coordinates"": [
        [
          [
            [139.7792162750725, 35.68458160287635, 39.850843770853956],
            [139.7793155292513, 35.68452053448293, 39.850843770853956],
            [139.7793155292513, 35.68452053448293, 68.45184377085396],
            [139.7792162750725, 35.68458160287635, 68.45184377085396],
            [139.7792162750725, 35.68458160287635, 39.850843770853956]
          ]
        ]
      ]
    },
    {
      ""gmlid"": ""ID_50b3af62-f738-4847-84d8-01679f7486d9"",
      ""coordinates"": [
        [
          [
            [139.77932576356716, 35.6845150857786, 39.82884377085396],
            [139.77938305031665, 35.68447962447881, 39.82884377085396],
            [139.77938305031665, 35.68447962447881, 75.11884377085396],
            [139.77932576356716, 35.6845150857786, 75.11884377085396],
            [139.77932576356716, 35.6845150857786, 39.82884377085396]
          ]
        ]
      ]
    },
    {
      ""gmlid"": ""ID_c88be3cf-f9a0-4bae-bd2c-18e074b7fecd"",
      ""coordinates"": [
        [
          [
            [139.7793921250045, 35.68447351724814, 39.82000476982577],
            [139.77951348438228, 35.684398938023946, 39.82000476982577],
            [139.77951348438228, 35.684398938023946, 68.56300476982577],
            [139.7793921250045, 35.68447351724814, 68.56300476982577],
            [139.7793921250045, 35.68447351724814, 39.82000476982577]
          ]
        ]
      ]
    },
    {
      ""gmlid"": ""ID_1fc81b77-326c-4914-8850-d1842c0208d1"",
      ""coordinates"": [
        [
          [
            [139.7790720334719, 35.6846736580886, 39.915400767100785],
            [139.77912972918858, 35.68463826019069, 39.915400767100785],
            [139.77912972918858, 35.68463826019069, 65.17040076710077],
            [139.7790720334719, 35.6846736580886, 65.17040076710077],
            [139.7790720334719, 35.6846736580886, 39.915400767100785]
          ]
        ]
      ]
    },
    {
      ""gmlid"": ""ID_ab4c416d-b578-43a4-9322-fd1ce74192b4"",
      ""coordinates"": [
        [
          [
            [139.7790164429172, 35.68470013333374, 39.93440076710078],
            [139.77906374827674, 35.684672212160265, 39.93440076710078],
            [139.77906374827674, 35.684672212160265, 69.97240076710078],
            [139.7790164429172, 35.68470013333374, 69.97240076710078],
            [139.7790164429172, 35.68470013333374, 39.93440076710078]
          ]
        ]
      ]
    }
  ]
}
";
            var detectedSurfaces = new MockSurfaces { DetectedSurfaces = new List<ISurfaceModel>() };

            var model = JObject.Parse(json);
            
            if (model["surfaces"] == null)
            {
                return detectedSurfaces;
            }
            
            foreach (var surface in model["surfaces"])
            {
                var detectedSurface = new MockSurface();

                if (surface["gmlid"] != null && surface["coordinates"] != null)
                {
                    detectedSurface.GmlId = surface["gmlid"].ToString();

                    detectedSurface.Coordinates = surface["coordinates"].SelectMany(root => root
                            .Select(coords => coords
                                .Select(coord => coord
                                    .Select(n => (double)n).ToList()).ToList()))
                        .ToList();
                }
                    
                detectedSurfaces.DetectedSurfaces.Add(detectedSurface);
            }
            
            return detectedSurfaces;
        }
    }
}