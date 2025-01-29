using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    public class ValidationDialogView : MonoBehaviour
    {
        [SerializeField] private ValidationDialogIconSprite[] iconSprites;
        [SerializeField] private GameObject rootObject;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text leftText;
        [SerializeField] private Image leftIconImage;
        [SerializeField] private TMP_Text rightText;
        [SerializeField] private Image rightIconImage;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_Text cancelButtonText;
        [SerializeField] private TMP_Text confirmButtonText;

        /// <summary>
        /// アイコンのスプライト
        /// </summary>
        public ValidationDialogIconSprite[] IconSprites
            => iconSprites;

        /// <summary>
        /// ルートオブジェクト
        /// </summary>
        public GameObject RootObject
            => rootObject;

        /// <summary>
        /// アイコン画像
        /// </summary>
        public Image IconImage
            => iconImage;

        /// <summary>
        /// タイトルテキスト
        /// </summary>
        public TMP_Text TitleText
            => titleText;

        /// <summary>
        /// 左テキスト
        /// </summary>
        public TMP_Text LeftText
            => leftText;

        /// <summary>
        /// 右アイコン画像
        /// </summary>
        public Image LeftIconImage
            => leftIconImage;

        /// <summary>
        /// 右テキスト
        /// </summary>
        public TMP_Text RightText
            => rightText;

        public Image RightIconImage
            => rightIconImage;

        /// <summary>
        /// 説明テキスト
        /// </summary>
        public TMP_Text DescriptionText
            => descriptionText;

        /// <summary>
        /// キャンセルボタン
        /// </summary>
        public Button CancelButton
            => cancelButton;

        /// <summary>
        /// 確認ボタン
        /// </summary>
        public Button ConfirmButton
            => confirmButton;

        /// <summary>
        /// キャンセルボタンのテキスト
        /// </summary>
        public TMP_Text CancelButtonText
            => cancelButtonText;

        /// <summary>
        /// 確認ボタンのテキスト
        /// </summary>
        public TMP_Text ConfirmButtonText
            => confirmButtonText;
    }
}