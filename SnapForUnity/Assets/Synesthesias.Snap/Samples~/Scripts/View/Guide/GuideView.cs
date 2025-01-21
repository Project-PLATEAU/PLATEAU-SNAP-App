using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 利用ガイド画面のView
    /// </summary>
    public class GuideView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        /// <summary>
        /// 閉じるボタン
        /// </summary>
        public Button CloseButton
            => closeButton;
    }
}