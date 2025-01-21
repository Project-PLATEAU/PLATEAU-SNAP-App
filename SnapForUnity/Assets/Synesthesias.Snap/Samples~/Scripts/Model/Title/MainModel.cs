namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// メインシーンのModel
    /// </summary>
    public class MainModel
    {
        private readonly SceneModel sceneModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainModel(SceneModel sceneModel)
        {
            this.sceneModel = sceneModel;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public void Start()
        {
            sceneModel.Transition(SceneNameDefine.Guide);
        }
    }
}