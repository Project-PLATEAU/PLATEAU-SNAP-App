using System;
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
        [SerializeField] private Material selectableMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private DetectionTouchView touchView;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Button menuButton;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;
        [SerializeField] private Transform meshParent;

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
        /// 選択時のMaterial
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

        /// <summary>
        /// メッシュを配置する親オブジェクト
        /// </summary>
        [Obsolete("ARAnchorやARGespatialAnchorを使用してください")]
        public Transform MeshParent
            => meshParent;
    }
}