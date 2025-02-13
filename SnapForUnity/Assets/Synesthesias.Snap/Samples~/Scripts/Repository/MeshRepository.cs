using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// メッシュ情報を管理するリポジトリ
    /// 検出されたメッシュや選択されたメッシュの情報を管理する
    /// </summary>
    public class MeshRepository
    {
        private readonly Subject<GameObject> selectedObjectSubject = new();
        private readonly Subject<bool> selectedSubject = new();
        private readonly Material detectedMaterial;
        private readonly Material selectedMaterial;
        private IMobileDetectionMeshView previousDetectedMeshView;
        private IMobileDetectionMeshView previousSelectedMeshView;

        /// <summary>
        /// 検出された建物が選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => selectedSubject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeshRepository(
            Material detectedMaterial,
            Material selectedMaterial)
        {
            this.detectedMaterial = detectedMaterial;
            this.selectedMaterial = selectedMaterial;
        }

        /// <summary>
        /// クリア
        /// 検出や選択されたメッシュをクリアする
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

        /// <summary>
        /// 検出されたメッシュのViewを設定する
        /// </summary>
        public void SetMesh(IMobileDetectionMeshView meshView)
        {
            var previousDetectedGameObject = previousDetectedMeshView?.GetGameObject();

            if (previousDetectedGameObject)
            {
                Object.Destroy(previousDetectedGameObject);
            }

            previousDetectedMeshView = meshView;
            OnSubscribeMesh(meshView);
        }

        /// <summary>
        /// メッシュのGameObjectを選択する
        /// </summary>
        public void SelectObject(GameObject gameObject)
        {
            selectedObjectSubject.OnNext(gameObject);
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
    }
}