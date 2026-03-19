using Google.XR.ARCoreExtensions;
using System;
using UnityEngine.XR.ARSubsystems;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialの精度を取得・管理するModel
    /// </summary>
    public class GeospatialAccuracyModel
    {
        private readonly AREarthManager earthManager;
        private readonly GeospatialMainLoopModel mainLoopModel;
        private readonly GeospatialAccuracyThresholdModel thresholdModel;
        
        // 精度管理用の変数
        private double lastOrientationYawAccuracy = double.MaxValue;
        private double lastHorizontalAccuracy = double.MaxValue;
        private double lastVerticalAccuracy = double.MaxValue;
        private bool isFirstAccuracyCheck = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialAccuracyModel(
            AREarthManager earthManager,
            GeospatialMainLoopModel mainLoopModel,
            GeospatialAccuracyThresholdModel thresholdModel)
        {
            this.earthManager = earthManager;
            this.mainLoopModel = mainLoopModel;
            this.thresholdModel = thresholdModel;
        }

        /// <summary>
        /// 精度を取得する
        /// </summary>
        /// <returns>精度の結果</returns>
        public GeospatialAccuracyResult GetAccuracy()
        {
            if (!mainLoopModel.State.IsReady)
            {
                return new GeospatialAccuracyResult(
                    mainLoopState: mainLoopModel.State);
            }

            // Earthのトラッキングが取れていない場合はLow扱い
            if (earthManager.EarthTrackingState != TrackingState.Tracking)
            {
                return new GeospatialAccuracyResult(
                    mainLoopState: mainLoopModel.State,
                    accuracyState: GeospatialAccuracyState.LowAccuracy);
            }

            var cameraGeospatialPose = earthManager.CameraGeospatialPose;

            if (cameraGeospatialPose.OrientationYawAccuracy > thresholdModel.HeadingThreshold
                || cameraGeospatialPose.HorizontalAccuracy > thresholdModel.HorizontalAccuracyThreshold
                || cameraGeospatialPose.VerticalAccuracy > thresholdModel.VerticalAccuracyThreshold)
            {
                return new GeospatialAccuracyResult(
                    mainLoopState: mainLoopModel.State,
                    accuracyState: GeospatialAccuracyState.LowAccuracy);
            }

            return new GeospatialAccuracyResult(
                mainLoopState: mainLoopModel.State,
                accuracyState: GeospatialAccuracyState.HighAccuracy);
        }

        /// <summary>
        /// VPSの精度が向上しているかチェックし、向上していれば記録を更新する
        /// </summary>
        /// <returns>精度が向上していればtrue、そうでなければfalse</returns>
        public bool CheckAndUpdateAccuracyIfImproved()
        {

            if (!mainLoopModel.State.IsReady)
            {
                return false;
            }

            var currentPose = earthManager.CameraGeospatialPose;
            
            // 初回の場合は精度を記録してfalseを返す
            if (isFirstAccuracyCheck)
            {
                lastOrientationYawAccuracy = currentPose.OrientationYawAccuracy;
                lastHorizontalAccuracy = currentPose.HorizontalAccuracy;
                lastVerticalAccuracy = currentPose.VerticalAccuracy;
                isFirstAccuracyCheck = false;
                return false;
            }

            // 精度が改善されているかチェック
            var isOrientationAccuracyImproved = currentPose.OrientationYawAccuracy < lastOrientationYawAccuracy;
            var isHorizontalAccuracyImproved = currentPose.HorizontalAccuracy < lastHorizontalAccuracy;
            var isVerticalAccuracyImproved = currentPose.VerticalAccuracy < lastVerticalAccuracy;

            if (isOrientationAccuracyImproved || isHorizontalAccuracyImproved)
            {
                // 精度を更新
                lastOrientationYawAccuracy = currentPose.OrientationYawAccuracy;
                lastHorizontalAccuracy = currentPose.HorizontalAccuracy;
                lastVerticalAccuracy = currentPose.VerticalAccuracy;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 現在記録されている精度情報を取得する
        /// </summary>
        /// <returns>方位精度、水平精度、垂直精度</returns>
        public (double OrientationYawAccuracy, double HorizontalAccuracy, double VerticalAccuracy) GetLastKnownAccuracy()
        {
            return (lastOrientationYawAccuracy, lastHorizontalAccuracy, lastVerticalAccuracy);
        }
    }
}