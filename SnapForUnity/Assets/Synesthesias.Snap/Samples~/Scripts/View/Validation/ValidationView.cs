using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    public class ValidationView : MonoBehaviour
    {
        [SerializeField] private RawImage capturedRawImage;

        /// <summary>
        /// 撮影した画像のRawImage
        /// </summary>
        public RawImage CapturedRawImage
            => capturedRawImage;
    }
}