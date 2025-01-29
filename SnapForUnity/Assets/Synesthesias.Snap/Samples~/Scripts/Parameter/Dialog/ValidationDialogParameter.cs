namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 判定ダイアログのパラメータ
    /// </summary>
    public class ValidationDialogParameter
    {
        /// <summary>
        /// 左のバリデーションテキスト
        /// </summary>
        public readonly string LeftValidationText;

        /// <summary>
        /// 右のバリデーションテキスト
        /// </summary>
        public readonly string RightValidationText;

        /// <summary>
        /// キャンセルボタンテキスト
        /// </summary>
        public readonly string CancelButtonText;

        /// <summary>
        /// 確認ボタンテキスト
        /// </summary>
        public readonly string ConfirmButtonText;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationDialogParameter(
            string leftValidationText,
            string rightValidationText,
            string cancelButtonText,
            string confirmButtonText)
        {
            LeftValidationText = leftValidationText;
            RightValidationText = rightValidationText;
            CancelButtonText = cancelButtonText;
            ConfirmButtonText = confirmButtonText;
        }
    }
}