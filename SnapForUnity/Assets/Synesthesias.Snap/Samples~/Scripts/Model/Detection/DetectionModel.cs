namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出シーンのModel
    /// </summary>
    public class DetectionModel
    {
        private readonly SceneModel sceneModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionModel(SceneModel sceneModel)
        {
            this.sceneModel = sceneModel;
        }

        /// <summary>
        /// 戻る
        /// </summary>
        public void Back()
        {
            sceneModel.Transition(SceneNameDefine.Main);
        }
    }
}