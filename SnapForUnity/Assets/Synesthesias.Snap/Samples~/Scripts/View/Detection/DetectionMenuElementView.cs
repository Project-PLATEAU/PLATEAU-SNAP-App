using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    public class DetectionMenuElementView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;

        /// <summary>
        /// ボタン
        /// </summary>
        public Button Button
            => button;

        /// <summary>
        /// テキスト
        /// </summary>
        public TMP_Text Text
            => text;
    }
}