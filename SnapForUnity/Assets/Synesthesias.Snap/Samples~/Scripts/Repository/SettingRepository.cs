namespace Synesthesias.Snap.Sample
{
    public class SettingRepository
    {
        private bool isAngleValid;
        private bool isSurfaceValid;

        /// <summary>
        /// 角度が正しいかどうかを設定
        /// </summary>
        public void SetIsAngleValid(bool isAngleValid)
        {
            this.isAngleValid = isAngleValid;
        }

        /// <summary>
        /// 面が正しいかどうかを設定
        /// </summary>
        public void SetIsSurfaceValid(bool isSurfaceValid)
        {
            this.isSurfaceValid = isSurfaceValid;
        }

        /// <summary>
        /// 角度が正しいかどうかを取得
        /// </summary>
        public bool GetIsAngleValid()
        {
            return isAngleValid;
        }

        /// <summary>
        /// 面が正しいかどうかを取得
        /// </summary>
        public bool GetIsSurfaceValid()
        {
            return isSurfaceValid;
        }
    }
}