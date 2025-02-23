using Cysharp.Threading.Tasks;
using System;
using Google.XR.ARCoreExtensions;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// GeospatialのメインループのModel
    /// </summary>
    public class GeospatialMainLoopModel
    {
        private readonly ARSession arSession;
        private readonly AREarthManager earthManager;
        private readonly ARCoreExtensions arCoreExtensions;

        /// <summary>
        /// 状態
        /// </summary>
        public readonly GeospatialMainLoopState State = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialMainLoopModel(
            ARSession arSession,
            AREarthManager earthManager,
            ARCoreExtensions arCoreExtensions)
        {
            this.arSession = arSession;
            this.earthManager = earthManager;
            this.arCoreExtensions = arCoreExtensions;
        }

        /// <summary>
        /// フレームレートを60にする
        /// </summary>
        public static void SetFrameRateAs60()
        {
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// 縦画面に固定する
        /// </summary>
        public static void LockToPortrait()
        {
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.Portrait;
        }

        /// <summary>
        /// 有効化
        /// </summary>
        public async UniTask EnableAsync(CancellationToken cancellationToken)
        {
            State.SetStateType(GeospatialMainLoopStateType.Enabled);

            var isArSessionStarted = false;

            while (!isArSessionStarted)
            {
                isArSessionStarted = await StartARSessionAsync(cancellationToken);
            }

            var isLocationServiceStarted = false;

            while (!isLocationServiceStarted)
            {
                isLocationServiceStarted = await StartLocationServiceAsync(cancellationToken);
            }

            var isArSessionAvailable = false;

            while (!isArSessionAvailable)
            {
                isArSessionAvailable = await CheckAvailabilityAsync(cancellationToken);
            }

            var isVpsAvailable = false;

            while (!isVpsAvailable)
            {
                isVpsAvailable = await CheckVpsAvailabilityAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 無効化
        /// </summary>
        public void Disable()
        {
#if UNITY_IOS
            Input.location.Stop();
#endif
            State.SetStateType(GeospatialMainLoopStateType.Disabled);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public async UniTask MainLoopAsync(CancellationToken cancellationToken)
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);

            if (ARSession.state is not ARSessionState.Ready
                and not ARSessionState.SessionInitializing
                and not ARSessionState.SessionTracking)
            {
                throw new InvalidOperationException($"ARSession.state: {ARSession.state}");
            }

            if (!await IsSupportedAsync(cancellationToken))
            {
                return;
            }

            await EnableGeospatialAsync(cancellationToken);

            State.SetEarthState(earthManager.EarthState);

            if (earthManager.EarthState != EarthState.Enabled)
            {
                State.SetStateType(GeospatialMainLoopStateType.EarthNotReady);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                return;
            }

#if UNITY_IOS
            var isSessionReady = ARSession.state == ARSessionState.SessionTracking &&
                                 Input.location.status == LocationServiceStatus.Running;
#else
            var isSessionReady = ARSession.state == ARSessionState.SessionTracking;
#endif

            if (isSessionReady)
            {
                State.SetStateType(GeospatialMainLoopStateType.Ready);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
            }
        }

        private async UniTask<bool> StartARSessionAsync(CancellationToken cancellationToken)
        {
            try
            {
                State.SetStateType(GeospatialMainLoopStateType.ARSessionResetting);
                arSession.Reset();

                await UniTask.WaitForSeconds(1F, cancellationToken: cancellationToken);

                return arSession.subsystem is { running: true };
            }
            catch
            {
                throw new GeospatialMainLoopException(
                    stateType: GeospatialMainLoopStateType.ARSessionResetFailed);
            }
        }

        private async UniTask<bool> StartLocationServiceAsync(CancellationToken cancellationToken)
        {
            if (!Input.location.isEnabledByUser)
            {
                State.SetStateType(GeospatialMainLoopStateType.LocationServiceDisabledByUser);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);

                throw new GeospatialMainLoopException(
                    stateType: GeospatialMainLoopStateType.LocationServiceDisabledByUser);
            }

            State.SetStateType(GeospatialMainLoopStateType.LocationServiceInitializing);
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);

            try
            {
                Input.location.Start();

                await UniTask.WaitWhile(
                    () => Input.location.status == LocationServiceStatus.Initializing,
                    cancellationToken: cancellationToken);

                if (Input.location.status != LocationServiceStatus.Running)
                {
                    State.SetStateType(GeospatialMainLoopStateType.LocationServiceFailed);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    Input.location.Stop();
                    return false;
                }

                State.SetStateType(GeospatialMainLoopStateType.LocationServiceRunning);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                return true;
            }
            catch
            {
                State.SetStateType(GeospatialMainLoopStateType.LocationServiceFailed);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                Input.location.Stop();
                throw new GeospatialMainLoopException(stateType: State.StateType);
            }
        }

        private async UniTask<bool> CheckAvailabilityAsync(CancellationToken cancellationToken)
        {
            switch (ARSession.state)
            {
                case ARSessionState.Unsupported:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionAvailabilityUnsupported);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    throw new GeospatialMainLoopException(GeospatialMainLoopStateType.ARSessionAvailabilityUnsupported);
                case ARSessionState.None:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionAvailabilityCheckInProgress);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);

                    await ARSession.CheckAvailability()
                        .ToUniTask(cancellationToken: cancellationToken);

                    return false;
                case ARSessionState.CheckingAvailability:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionAvailabilityCheckInProgress);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return false;
                case ARSessionState.NeedsInstall:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionInstalling);

                    await ARSession.Install()
                        .ToUniTask(cancellationToken: cancellationToken);

                    return false;
                case ARSessionState.Installing:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionInstalling);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return false;
                case ARSessionState.Ready:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionReady);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return true;
                case ARSessionState.SessionInitializing:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionInitializing);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return true;
                case ARSessionState.SessionTracking:
                    State.SetStateType(GeospatialMainLoopStateType.ARSessionTracking);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return true;
                default:
                    throw new NotSupportedException($"未実装のARSession.state: {ARSession.state}");
            }
        }

        private async UniTask<bool> CheckVpsAvailabilityAsync(CancellationToken cancellationToken)
        {
            State.SetStateType(GeospatialMainLoopStateType.VpsAvailabilityChecking);
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);

            var location = Input.location.lastData;

            var vpsAvailabilityPromise = AREarthManager.CheckVpsAvailabilityAsync(
                latitude: location.latitude,
                longitude: location.longitude);

            await vpsAvailabilityPromise.ToUniTask(cancellationToken: cancellationToken);
            var vpsAvailability = vpsAvailabilityPromise.Result;

            if (vpsAvailability == VpsAvailability.Available)
            {
                State.SetStateType(GeospatialMainLoopStateType.VpsAvailable);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                return true;
            }

            State.SetStateType(GeospatialMainLoopStateType.VpsNotAvailable);
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
            return false;
        }

        private static bool IsSessionReady()
        {
            var result = ARSession.state == ARSessionState.CheckingAvailability
                         || ARSession.state == ARSessionState.Ready
                         || ARSession.state == ARSessionState.SessionInitializing
                         || ARSession.state == ARSessionState.SessionTracking;

            return result;
        }

        private static bool IsLocationServiceReady()
        {
#if UNITY_IOS
            return Input.location.status == LocationServiceStatus.Running;
#else
            return true;
#endif
        }

        private async UniTask<bool> IsSupportedAsync(CancellationToken cancellationToken)
        {
            var featureSupport = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
            State.SetFeatureSupported(featureSupport);
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);

            switch (featureSupport)
            {
                case FeatureSupported.Unknown:
                    Debug.Log("UpdateAsync(12)");
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return false;
                case FeatureSupported.Unsupported:
                    Debug.Log("UpdateAsync(13)");
                    State.SetStateType(GeospatialMainLoopStateType.NotSupported);
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
                    return false;
                case FeatureSupported.Supported:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask EnableGeospatialAsync(CancellationToken cancellationToken)
        {
            if (arCoreExtensions.ARCoreExtensionsConfig.GeospatialMode ==
                GeospatialMode.Enabled)
            {
                return;
            }

            State.SetStateType(GeospatialMainLoopStateType.GeospatialEnabling);
            arCoreExtensions.ARCoreExtensionsConfig.GeospatialMode = GeospatialMode.Enabled;
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
            await UniTask.WaitForSeconds(3.0F, cancellationToken: cancellationToken);
        }
    }
}