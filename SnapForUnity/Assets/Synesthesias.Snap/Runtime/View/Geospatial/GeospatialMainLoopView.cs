using R3;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// GeospatialのメインループのView
    /// </summary>
    public class GeospatialMainLoopView : MonoBehaviour
    {
        private readonly Subject<Unit> awakeSubject = new();
        [SerializeField] private bool isLockPortrait;
        [SerializeField] private bool isFrameRateAs60;

        /// <summary>
        /// 縦画面に固定するかどうか
        /// </summary>
        public bool IsLockPortrait
            => isLockPortrait;

        /// <summary>
        /// フレームレートを60にするかどうか
        /// </summary>
        public bool IsFrameRateAs60
            => isFrameRateAs60;

        /// <summary>
        /// AwakeのObservable
        /// </summary>
        public Observable<Unit> AwakeObservable()
            => awakeSubject;

        private void Awake()
        {
            awakeSubject.OnNext(Unit.Default);
        }
    }
}