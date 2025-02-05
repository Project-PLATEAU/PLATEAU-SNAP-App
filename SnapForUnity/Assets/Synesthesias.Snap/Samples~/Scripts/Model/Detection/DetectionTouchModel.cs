using R3;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面の画面タップ関連のModel
    /// </summary>
    public class DetectionTouchModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private readonly Subject<bool> selectedSubject = new();
        private readonly Subject<GameObject> selectedObjectSubject = new();
        private readonly ReactiveProperty<bool> isTapToCreateAnchorProperty = new(false);
        private readonly DetectionMenuModel menuModel;
        private readonly Material detectedMaterial;
        private readonly Material selectedMaterial;
        private IMobileDetectionMeshView previousDetectedMeshView;
        private IMobileDetectionMeshView previousSelectedMeshView;

        /// <summary>
        /// タップでアンカーを作成するか
        /// </summary>
        public bool IsTapToCreateAnchor
            => isTapToCreateAnchorProperty.Value;

        /// <summary>
        /// 検出された建物が選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => selectedSubject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionTouchModel(
            DetectionMenuModel menuModel,
            Material detectedMaterial,
            Material selectedMaterial)
        {
            this.menuModel = menuModel;
            this.detectedMaterial = detectedMaterial;
            this.selectedMaterial = selectedMaterial;
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
            var previousDetectedGameObject = previousDetectedMeshView?.GetGameObject();

            if (previousDetectedGameObject)
            {
                Object.Destroy(previousDetectedGameObject);
            }

            OnSubscribeMesh(meshView);
            previousDetectedMeshView = meshView;
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            if (previousDetectedMeshView == null)
            {
                return;
            }

            Object.Destroy(previousDetectedMeshView.GetGameObject());
            previousDetectedMeshView = null;
            previousSelectedMeshView = null;
            selectedSubject.OnNext(false);
        }

        private void CreateMenu()
        {
            var tapTopPlaceAnchorMenuElement = CreateTapToCreateAnchorMenuElementModel();
            menuModel.AddElement(tapTopPlaceAnchorMenuElement);
        }

        private void OnSubscribeMesh(IMobileDetectionMeshView meshView)
        {
            var gameObject = meshView.GetGameObject();

            selectedObjectSubject
                .Where(selectedObject => selectedObject == gameObject)
                .Subscribe(_ => OnMeshViewSelected(meshView))
                .AddTo(gameObject);
        }

        private void OnMeshViewSelected(IMobileDetectionMeshView meshView)
        {
            if (OnSameViewSelected(meshView))
            {
                return;
            }

            OnDifferentViewSelected(meshView);
        }

        private bool OnSameViewSelected(IMobileDetectionMeshView meshView)
        {
            if (meshView != previousSelectedMeshView)
            {
                return false;
            }

            meshView.MeshRenderer.material = detectedMaterial;
            previousSelectedMeshView = null;
            selectedSubject.OnNext(false);
            return true;
        }

        private void OnDifferentViewSelected(IMobileDetectionMeshView meshView)
        {
            if (previousSelectedMeshView != null)
            {
                previousSelectedMeshView.MeshRenderer.material = detectedMaterial;
            }

            meshView.MeshRenderer.material = selectedMaterial;
            previousSelectedMeshView = meshView;
            selectedSubject.OnNext(true);
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
            selectedObjectSubject.OnNext(hitGameObject);
        }
    }
}