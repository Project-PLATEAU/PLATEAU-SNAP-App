using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Synesthesias.Snap.Runtime
{
    public class GeospatialAnchorModel
    {
        private readonly GeospatialAccuracyModel accuracyModel;
        private readonly ARAnchorManager arAnchorManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAnchorModel(
            GeospatialAccuracyModel accuracyModel,
            ARAnchorManager arAnchorManager)
        {
            this.accuracyModel = accuracyModel;
            this.arAnchorManager = arAnchorManager;
        }

        /// <summary>
        /// ARGeospatialAnchorを作成する
        /// </summary>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">経度</param>
        /// <param name="altitude">高度</param>
        /// <param name="eunRotation">EUN回転</param>
        public GeospatialAnchorResult CreateAnchor(
            double latitude,
            double longitude,
            double altitude,
            Quaternion eunRotation)
        {
            var accuracyResult = accuracyModel.GetAccuracy();

            if (!accuracyResult.IsSuccess)
            {
                return new GeospatialAnchorResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState);
            }

            try
            {
                var geospatialAnchor = arAnchorManager.AddAnchor(
                    latitude: latitude,
                    longitude: longitude,
                    altitude: altitude,
                    eunRotation: eunRotation);

                var result = new GeospatialAnchorResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialAnchorResultType.Success,
                    anchor: geospatialAnchor);

                return result;
            }
            catch
            {
                return new GeospatialAnchorResult(
                    mainLoopState: accuracyResult.MainLoopState,
                    accuracyState: accuracyResult.AccuracyState,
                    resultType: GeospatialAnchorResultType.Failed);
            }
        }
    }
}