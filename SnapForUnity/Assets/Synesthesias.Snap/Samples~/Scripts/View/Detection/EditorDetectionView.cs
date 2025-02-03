using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView(エディタ用)
    /// </summary>
    public class EditorDetectionView : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button cameraDeviceToggleButton;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public Button BackButton
            => backButton;

        /// <summary>
        /// カメラデバイス切り替えボタン
        /// </summary>
        public Button CameraDeviceToggleButton
            => cameraDeviceToggleButton;

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