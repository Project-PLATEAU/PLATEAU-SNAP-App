using Google.XR.ARCoreExtensions;
using System.Text;
using UnityEngine.XR.ARSubsystems;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// GeospatialのデバッグModel
    /// </summary>
    public class GeospatialDebugModel
    {
        private readonly StringBuilder stringBuilder = new();
        private readonly AREarthManager arEarthManager;
        private readonly GeospatialMainLoopModel geospatialMainLoopModel;
        private readonly GeospatialAccuracyModel geospatialAccuracyModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialDebugModel(
            AREarthManager arEarthManager,
            GeospatialMainLoopModel geospatialMainLoopModel,
            GeospatialAccuracyModel geospatialAccuracyModel)
        {
            this.arEarthManager = arEarthManager;
            this.geospatialMainLoopModel = geospatialMainLoopModel;
            this.geospatialAccuracyModel = geospatialAccuracyModel;
        }

        /// <summary>
        /// デバッグテキストを取得する
        /// </summary>
        public string GetDebugText(GeospatialPose geospatialPose)
        {
            var result = string.Empty;
            var mainLoopState = geospatialMainLoopModel.State;

            stringBuilder
                .AppendLine($"StateType: {mainLoopState.StateType.ToMessage()}")
                .AppendLine($"FeatureSupported: {mainLoopState.FeatureSupported}")
                .AppendLine($"EarthState: {mainLoopState.EarthState}")
                .AppendLine($"ARSessionState: {mainLoopState.ARSessionState}");

            if (arEarthManager.EarthTrackingState != TrackingState.Tracking)
            {
                stringBuilder
                    .AppendLine("Earth Tracking State: Not Tracking");

                result = stringBuilder.ToString();
                stringBuilder.Clear();
                return result;
            }

            var accuracyResult = geospatialAccuracyModel.GetAccuracy();

            stringBuilder
                .AppendLine($"Accuracy: {accuracyResult.AccuracyState.ToMessage()}")
                .Append("Horizontal Accuracy: ")
                .Append(geospatialPose.HorizontalAccuracy.ToString("F6"))
                .AppendLine()
                .Append("Vertical Accuracy: ")
                .Append(geospatialPose.VerticalAccuracy.ToString("F2"))
                .AppendLine();

            stringBuilder
                .AppendLine($"Latitude/Longitude: ")
                .Append(geospatialPose.Latitude.ToString("F6"))
                .Append("/")
                .Append(geospatialPose.Longitude.ToString("F6"))
                .AppendLine()
                .Append("Altitude: ")
                .Append(geospatialPose.Altitude.ToString("F2"))
                .AppendLine()
                .Append("Heading: ")
                .Append(geospatialPose.EunRotation.ToString("F1"))
                .AppendLine()
                .Append("Heading Accuracy: ")
                .Append(geospatialPose.OrientationYawAccuracy.ToString("F1"));

            result = stringBuilder.ToString();
            stringBuilder.Clear();
            return result;
        }
    }
}