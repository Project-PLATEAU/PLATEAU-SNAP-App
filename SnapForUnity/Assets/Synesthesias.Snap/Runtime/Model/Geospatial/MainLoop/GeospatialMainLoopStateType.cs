using System;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatialのメインループの状態
    /// </summary>
    public enum GeospatialMainLoopStateType
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 有効化
        /// </summary>
        Enabled,

        /// <summary>
        /// 無効化
        /// </summary>
        Disabled,

        /// <summary>
        /// ARセッションのリセット中
        /// </summary>
        ARSessionResetting,

        /// <summary>
        /// ARセッションのリセット失敗
        /// </summary>
        ARSessionResetFailed,

        /// <summary>
        /// 位置サービスがユーザーによって無効化されている
        /// </summary>
        LocationServiceDisabledByUser,

        /// <summary>
        /// 位置サービスの初期化中
        /// </summary>
        LocationServiceInitializing,

        /// <summary>
        /// 位置サービスの初期化失敗
        /// </summary>
        LocationServiceFailed,

        /// <summary>
        /// 位置サービスが実行中
        /// </summary>
        LocationServiceRunning,

        /// <summary>
        /// ARセッションのAvailabilityチェック中
        /// </summary>
        ARSessionAvailabilityCheckInProgress,

        /// <summary>
        /// ARセッションがデバイスでサポートされていない
        /// </summary>
        ARSessionAvailabilityUnsupported,

        /// <summary>
        /// ARセッションがデバイスでサポートされていて準備完了
        /// </summary>
        ARSessionAvailabilitySupportedAndReady,

        /// <summary>
        /// VPSのAvailabilityチェック中
        /// </summary>
        VpsAvailabilityChecking,

        /// <summary>
        /// VPSが利用できない
        /// </summary>
        VpsNotAvailable,

        /// <summary>
        /// VPSが利用できる
        /// </summary>
        VpsAvailable,

        /// <summary>
        /// ARセッションのインストール中
        /// </summary>
        ARSessionInstalling,

        /// <summary>
        /// セッションの準備完了
        /// </summary>
        ARSessionReady,

        /// <summary>
        /// セッションの初期化中
        /// </summary>
        ARSessionInitializing,

        /// <summary>
        /// セッションのトラッキング中
        /// </summary>
        ARSessionTracking,

        /// <summary>
        /// セッションの準備中
        /// </summary>
        ARSessionPreparing,

        /// <summary>
        /// ARCoreのセッションエラー
        /// </summary>
        ARSessionError,

        /// <summary>
        /// デバイスがサポートされていない
        /// </summary>
        NotSupported,

        /// <summary>
        /// Geospatialの有効化中
        /// </summary>
        GeospatialEnabling,

        /// <summary>
        /// Earthが準備されていない
        /// </summary>
        EarthNotReady,

        /// <summary>
        /// エラー
        /// </summary>
        Error,

        /// <summary>
        /// 準備完了
        /// </summary>
        Ready
    }

    /// <summary>
    /// Geospatialのメインループの状態の拡張メソッド
    /// </summary>
    public static class GeospatialMainLoopStateTypeExtensions
    {
        /// <summary>
        /// メッセージに変換する
        /// </summary>
        public static string ToMessage(this GeospatialMainLoopStateType stateType)
        {
            return stateType switch
            {
                GeospatialMainLoopStateType.None => "なし",
                GeospatialMainLoopStateType.Enabled => "有効化",
                GeospatialMainLoopStateType.Disabled => "無効化",
                GeospatialMainLoopStateType.ARSessionResetting => "ARセッションのリセット中",
                GeospatialMainLoopStateType.ARSessionResetFailed => "ARセッションのリセット失敗",
                GeospatialMainLoopStateType.LocationServiceDisabledByUser => "位置サービスがユーザーによって無効化されている",
                GeospatialMainLoopStateType.LocationServiceInitializing => "位置サービスの初期化中",
                GeospatialMainLoopStateType.LocationServiceFailed => "位置サービスの初期化失敗",
                GeospatialMainLoopStateType.LocationServiceRunning => "位置サービスが実行中",
                GeospatialMainLoopStateType.ARSessionAvailabilityCheckInProgress => "ARセッションのAvailabilityチェック中",
                GeospatialMainLoopStateType.ARSessionAvailabilityUnsupported => "ARセッションがデバイスでサポートされていない",
                GeospatialMainLoopStateType.ARSessionAvailabilitySupportedAndReady => "ARセッションがデバイスでサポートされていて準備完了",
                GeospatialMainLoopStateType.VpsAvailabilityChecking => "VPSのAvailabilityチェック中",
                GeospatialMainLoopStateType.VpsNotAvailable => "VPSが利用できない",
                GeospatialMainLoopStateType.VpsAvailable => "VPSが利用できる",
                GeospatialMainLoopStateType.ARSessionInstalling => "ARセッションのインストール中",
                GeospatialMainLoopStateType.ARSessionReady => "セッションの準備完了",
                GeospatialMainLoopStateType.ARSessionInitializing => "セッションの初期化中",
                GeospatialMainLoopStateType.ARSessionTracking => "セッションのトラッキング中",
                GeospatialMainLoopStateType.ARSessionPreparing => "セッションの準備中",
                GeospatialMainLoopStateType.ARSessionError => "ARCoreのセッションエラー",
                GeospatialMainLoopStateType.NotSupported => "デバイスがサポートされていない",
                GeospatialMainLoopStateType.GeospatialEnabling => "Geospatialの有効化中",
                GeospatialMainLoopStateType.EarthNotReady => "Earthが準備されていない",
                GeospatialMainLoopStateType.Error => "エラー",
                GeospatialMainLoopStateType.Ready => "準備完了",
                _ => throw new NotImplementedException($"未実装の状態: {stateType}")
            };
        }
    }
}