using R3;
using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面の画面タップ関連のModel
    /// </summary>
    public class DetectionTouchModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly ReactiveProperty<bool> isTapToCreateAnchorProperty = new(false);
        private readonly MeshRepository meshRepository;
        private readonly DetectionMenuModel menuModel;

        /// <summary>
        /// タップでアンカーを作成するか
        /// </summary>
        public bool IsTapToCreateAnchor
            => isTapToCreateAnchorProperty.Value;

        /// <summary>
        /// 検出された建物が選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => meshRepository.OnSelectedAsObservable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionTouchModel(
            MeshRepository meshRepository,
            DetectionMenuModel menuModel)
        {
            this.meshRepository = meshRepository;
            this.menuModel = menuModel;
            CreateMenu();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        /// <summary>
        /// 画面をタッチ
        /// </summary>
        public void TouchScreen(Camera camera, Vector2 screenPosition)
        {
            OnSelectAnchor(camera, screenPosition);
        }

        /// <summary>
        /// 検出されたメッシュのViewを設定する
        /// </summary>
        public void SetDetectedMeshView(IMobileDetectionMeshView meshView)
        {
            meshRepository.SetMesh(meshView);
        }

        private void CreateMenu()
        {
            var tapTopPlaceAnchorMenuElement = CreateTapToCreateAnchorMenuElementModel();
            menuModel.AddElement(tapTopPlaceAnchorMenuElement);
        }

        private DetectionMenuElementModel CreateTapToCreateAnchorMenuElementModel()
        {
            var result = new DetectionMenuElementModel(
                text: "タップでアンカー配置: ---",
                onClick: OnClickTapToCreateAnchor);

            isTapToCreateAnchorProperty
                .Subscribe(isTapToPlaceAnchor =>
                {
                    var stateText = isTapToPlaceAnchor ? "有効" : "無効";
                    var text = $"タップでアンカー配置: {stateText}";
                    result.TextProperty.Value = text;
                })
                .AddTo(disposable);

            return result;
        }

        private void OnClickTapToCreateAnchor()
        {
            var previous = isTapToCreateAnchorProperty.Value;
            var current = !previous;
            isTapToCreateAnchorProperty.Value = current;
        }

        private void OnSelectAnchor(Camera camera, Vector2 screenPosition)
        {
            var ray = camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit))
            {
                return;
            }

            var hitGameObject = hit.collider.gameObject;
            meshRepository.SelectObject(hitGameObject);
        }
    }
}