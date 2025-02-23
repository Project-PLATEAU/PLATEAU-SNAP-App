using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialのメインループの状態
    /// </summary>
    public class GeospatialMainLoopState
    {
        /// <summary>
        /// メインループの状態
        /// </summary>
        public GeospatialMainLoopStateType StateType { get; private set; }

        /// <summary>
        /// デバイスのサポート状況
        /// </summary>
        public FeatureSupported FeatureSupported { get; private set; }

        /// <summary>
        /// 位置サービスの状態
        /// </summary>
        public LocationServiceStatus LocationServiceStatus
            => Input.location.status;

        /// <summary>
        /// Earthの状態
        /// </summary>
        public EarthState EarthState { get; private set; }

        /// <summary>
        /// ARSessionの状態
        /// </summary>
        public ARSessionState ARSessionState
            => ARSession.state;

        /// <summary>
        /// 準備完了かどうか
        /// </summary>
        public bool IsReady
            => StateType == GeospatialMainLoopStateType.Ready;

        /// <summary>
        /// メインループの状態を設定する
        /// </summary>
        public void SetStateType(GeospatialMainLoopStateType stateType)
        {
            StateType = stateType;
        }

        /// <summary>
        /// デバイスのサポート状況を設定する
        /// </summary>
        public void SetFeatureSupported(FeatureSupported featureSupported)
        {
            FeatureSupported = featureSupported;
        }

        /// <summary>
        /// Earthの状態を設定する
        /// </summary>
        public void SetEarthState(EarthState earthState)
        {
            EarthState = earthState;
        }
    }
}