using UnityEngine;
using UnityEngine.EventSystems;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 画面タッチのModel
    /// </summary>
    public class ScreenTouchModel
    {
        /// <summary>
        /// タッチ数を取得
        /// </summary>
        public int GetTouchCount()
        {
            var result = Input.touchCount;
            return result;
        }

        /// <summary>
        /// タッチを取得
        /// </summary>
        public Touch GetTouch(int index)
        {
            var touch = Input.GetTouch(index: index);
            return touch;
        }

        /// <summary>
        /// 画面をタッチしているかどうか
        /// </summary>
        public bool IsTouchScreen(Touch touch)
        {
            if (!IsTouchBegan(touch))
            {
                return false;
            }

            var isPointerOverGame = IsPointerOverGameObject(touch);
            return !isPointerOverGame;
        }

        /// <summary>
        /// 画面のタッチが開始しているか
        /// </summary>
        private static bool IsTouchBegan(Touch touch)
        {
            var result = touch.phase == TouchPhase.Began;
            return result;
        }

        /// <summary>
        /// 別のGameObjectをタップしてしまっているか
        /// </summary>
        private static bool IsPointerOverGameObject(Touch touch)
        {
            var result = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
            return result;
        }
    }
}