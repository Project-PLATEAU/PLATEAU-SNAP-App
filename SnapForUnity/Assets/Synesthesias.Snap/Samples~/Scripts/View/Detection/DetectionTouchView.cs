using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Synesthesias.Snap.Sample
{
    public class DetectionTouchView : MonoBehaviour
    {
        private readonly Subject<Vector2> screenInputSubject = new();

        [SerializeField] private Camera targetCamera;

        /// <summary>
        /// 画面タッチのObservable
        /// </summary>
        public Observable<Vector2> OnScreenInputAsObservable()
            => screenInputSubject;

        /// <summary>
        /// ターゲットのカメラ
        /// </summary>
        public Camera TargetCamera
            => targetCamera;

        private void Update()
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                screenInputSubject.OnNext(Input.mousePosition);
            }
#else
            if (Input.touchCount <= 0)
            {
                return;
            }

            var touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            if (touch.phase == TouchPhase.Began)
            {
                screenInputSubject.OnNext(touch.position);
            }
#endif
        }
    }
}