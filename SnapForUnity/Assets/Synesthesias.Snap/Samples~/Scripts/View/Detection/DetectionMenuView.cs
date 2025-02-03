using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    public class DetectionMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject rootObject;
        [SerializeField] private Transform contentTransform;
        [SerializeField] private DetectionMenuElementView templateButton;
        [SerializeField] private Button backgroundButton;

        /// <summary>
        /// ルートオブジェクト
        /// </summary>
        public GameObject RootObject
            => rootObject;

        /// <summary>
        /// コンテンツのTransform
        /// </summary>
        public Transform ContentTransform
            => contentTransform;

        /// <summary>
        /// テンプレートのボタン
        /// </summary>
        public DetectionMenuElementView TemplateButton
            => templateButton;

        /// <summary>
        /// 背景のボタン
        /// </summary>
        public Button BackgroundButton
            => backgroundButton;
    }
}