namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 利用ガイドシーンのModel
    /// </summary>
    public class GuideModel
    {
        private readonly SceneModel sceneModel;
        private readonly PlatformModel platformModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GuideModel(
            SceneModel sceneModel,
            PlatformModel platformModel)
        {
            this.sceneModel = sceneModel;
            this.platformModel = platformModel;
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        public void Close()
        {
            var sceneName = platformModel.IsSupportedMobileDevice()
                ? SceneNameDefine.MobileDetection
                : SceneNameDefine.EditorDetection;

            sceneModel.Transition(sceneName);
        }
    }
}