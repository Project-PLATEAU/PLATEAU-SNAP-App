using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView(携帯端末)
    /// </summary>
    public class MobileDetectionView : MonoBehaviour
    {
        [SerializeField] private Material detectedMaterial;
        [SerializeField] private Material selectableMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private DetectionTouchView touchView;
        [SerializeField] private Camera arCamera;
        [SerializeField] private Button menuButton;
        [SerializeField] private GameObject geospatialObject;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;

        /// <summary>
        /// 検出時のMaterial
        /// </summary>
        public Material DetectedMaterial
            => detectedMaterial;

        /// <summary>
        /// 選択可能時のMaterial
        /// </summary>
        public Material SelectableMaterial
            => selectableMaterial;

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
        /// ARカメラ
        /// </summary>
        public Camera ArCamera
            => arCamera;

        /// <summary>
        /// メニューボタン
        /// </summary>
        public Button MenuButton
            => menuButton;

        /// <summary>
        /// Geospatial情報のオブジェクト
        /// </summary>
        public GameObject GeospatialObject
            => geospatialObject;

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