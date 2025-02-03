using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView(エディタ用)
    /// </summary>
    public class EditorDetectionView : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;

        /// <summary>
        /// メニューボタン
        /// </summary>
        public Button MenuButton
            => menuButton;

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