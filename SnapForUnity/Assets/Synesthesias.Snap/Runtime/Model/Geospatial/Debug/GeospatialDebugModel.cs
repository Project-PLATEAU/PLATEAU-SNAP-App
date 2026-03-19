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

            // 基本情報の表示
            stringBuilder
                .AppendLine($"StateType: {mainLoopState.StateType.ToMessage()}")
                .AppendLine($"FeatureSupported: {mainLoopState.FeatureSupported}")
                .AppendLine($"ARSessionState: {mainLoopState.ARSessionState}");

            // AREarthManagerの状態表示
            stringBuilder
                .AppendLine($"--- AREarthManager State ---")
                .AppendLine($"EarthState: {arEarthManager.EarthState}")
                .AppendLine($"EarthTrackingState: {arEarthManager.EarthTrackingState}");

            if (arEarthManager.EarthTrackingState != TrackingState.Tracking)
            {
                result = stringBuilder.ToString();
                stringBuilder.Clear();
                return result;
            }

            var accuracyResult = geospatialAccuracyModel.GetAccuracy();

            // 精度情報の表示
            stringBuilder
                .AppendLine($"--- Accuracy Information ---")
                .AppendLine($"Accuracy State: {accuracyResult.AccuracyState.ToMessage()}")
                .Append("Horizontal Accuracy: ")
                .Append(geospatialPose.HorizontalAccuracy.ToString("F6"))
                .Append(" m")
                .AppendLine()
                .Append("Vertical Accuracy: ")
                .Append(geospatialPose.VerticalAccuracy.ToString("F2"))
                .Append(" m")
                .AppendLine();

            // 位置情報の表示
            stringBuilder
                .AppendLine($"--- Position Information ---")
                .Append("Latitude/Longitude: ")
                .Append(geospatialPose.Latitude.ToString("F6"))
                .Append("/")
                .Append(geospatialPose.Longitude.ToString("F6"))
                .AppendLine()
                .Append("Altitude: ")
                .Append(geospatialPose.Altitude.ToString("F2"))
                .Append(" m")
                .AppendLine()
                .Append("Heading: ")
                .Append(geospatialPose.EunRotation.ToString("F1"))
                .Append("°")
                .AppendLine()
                .Append("Heading Accuracy: ")
                .Append(geospatialPose.OrientationYawAccuracy.ToString("F1"))
                .Append("°");

            result = stringBuilder.ToString();
            stringBuilder.Clear();
            return result;
        }
    }
}