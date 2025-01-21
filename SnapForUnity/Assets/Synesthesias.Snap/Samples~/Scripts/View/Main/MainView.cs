using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// メイン画面のView
    /// </summary>
    public class MainView : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        /// <summary>
        /// アプリ起動ボタン
        /// </summary>
        public Button StartButton
            => startButton;
    }
}