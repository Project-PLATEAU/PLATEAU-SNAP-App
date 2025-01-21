namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 利用ガイドシーンのModel
    /// </summary>
    public class GuideModel
    {
        private readonly SceneModel sceneModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GuideModel(SceneModel sceneModel)
        {
            this.sceneModel = sceneModel;
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        public void Close()
        {
            sceneModel.Transition(SceneNameDefine.Detection);
        }
    }
}