using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のView(携帯端末)
    /// </summary>
    public class MobileDetectionView : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private GameObject geospatialObject;
        [SerializeField] private RawImage cameraRawImage;
        [SerializeField] private Button cameraButton;

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