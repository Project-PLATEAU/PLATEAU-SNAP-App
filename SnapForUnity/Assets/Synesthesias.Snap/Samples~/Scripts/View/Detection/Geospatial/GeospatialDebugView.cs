using TMPro;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// Geospatialのデバッグ用View
    /// </summary>
    public class GeospatialDebugView : MonoBehaviour
    {
        [SerializeField] private TMP_Text debugText;

        /// <summary>
        /// デバッグ用テキスト
        /// </summary>
        public TMP_Text DebugText
            => debugText;
    }
}