using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    public class DetectionMenuModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
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
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
            disposable.Clear();
        }

        /// <summary>
        /// 要素を追加
        /// </summary>
        public void PopulateElements()
        {
            AddElement(new DetectionMenuElementModel(
                text: "利用ガイド",
                onClickAsync: OnClickGuideAsync));

            AddElement(new DetectionMenuElementModel(
                text: "アプリ再起動",
                onClickAsync: OnClickRebootAsync));
        }

        /// <summary>
        /// メニュー要素を追加
        /// </summary>
        public void AddElement(DetectionMenuElementModel element)
        {
            disposable.Add(element);
            addElementSubject.OnNext(element);
        }

        private async UniTask OnClickGuideAsync(CancellationToken cancellationToken)
        {
            sceneModel.Transition(SceneNameDefine.Guide);
            await UniTask.Yield();
        }

        private async UniTask OnClickRebootAsync(CancellationToken cancellationToken)
        {
            sceneModel.Transition(SceneNameDefine.Boot);
            await UniTask.Yield();
        }
    }
}