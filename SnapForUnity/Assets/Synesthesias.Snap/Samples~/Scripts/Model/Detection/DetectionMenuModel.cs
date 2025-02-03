using R3;

namespace Synesthesias.Snap.Sample
{
    public class DetectionMenuModel
    {
        private readonly Subject<DetectionMenuElementModel> addElementSubject = new();
        private readonly SceneModel sceneModel;

        /// <summary>
        /// メニューを表示するか
        /// </summary>
        public readonly ReactiveProperty<bool> IsVisibleProperty = new(false);

        /// <summary>
        /// メニューの要素を追加のObservable
        /// </summary>
        public Observable<DetectionMenuElementModel> OnElementAddedAsObservable()
            => addElementSubject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMenuModel(SceneModel sceneModel)
        {
            this.sceneModel = sceneModel;
        }

        /// <summary>
        /// 要素を追加
        /// </summary>
        public void PopulateElements()
        {
            AddElement(new DetectionMenuElementModel(
                text: "利用ガイド",
                onClick: OnClickGuide));

            AddElement(new DetectionMenuElementModel(
                text: "アプリ再起動",
                onClick: OnClickReboot));
        }

        /// <summary>
        /// メニュー要素を追加
        /// </summary>
        public void AddElement(DetectionMenuElementModel element)
        {
            addElementSubject.OnNext(element);
        }

        private void OnClickGuide()
        {
            sceneModel.Transition(SceneNameDefine.Guide);
        }

        private void OnClickReboot()
        {
            sceneModel.Transition(SceneNameDefine.Boot);
        }
    }
}