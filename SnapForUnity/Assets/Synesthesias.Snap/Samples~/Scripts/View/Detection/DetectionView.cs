using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView
    /// </summary>
    public class DetectionView : MonoBehaviour
    {
        [SerializeField] private Button backButton;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public Button BackButton
            => backButton;
    }
}