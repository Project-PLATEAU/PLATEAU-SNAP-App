using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView(エディタ用)
    /// </summary>
    public class EditorDetectionView : MonoBehaviour
    {
        [SerializeField] private Material detectedMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private DetectionTouchView touchView;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Button menuButton;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;

        /// <summary>
        /// 検出時のMaterial
        /// </summary>
        public Material DetectedMaterial
            => detectedMaterial;

        /// <summary>
        /// 選択中のMaterial
        /// </summary>
        public Material SelectedMaterial
            => selectedMaterial;

        /// <summary>
        /// タッチ関連のView
        /// </summary>
        public DetectionTouchView TouchView
            => touchView;

        /// <summary>
        /// メインカメラ
        /// </summary>
        public Camera MainCamera
            => mainCamera;

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