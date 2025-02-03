using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView(携帯端末)
    /// </summary>
    public class MobileDetectionView : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public Button BackButton
            => backButton;

        /// <summary>
        /// カメラのRawImage
        /// </summary>
        public RawImage CameraRawImage
            => cameraRawImage;

        /// <summary>
        /// カメラボタン
        /// </summary>
        public Button CameraButton
            => cameraButton;
    }
}