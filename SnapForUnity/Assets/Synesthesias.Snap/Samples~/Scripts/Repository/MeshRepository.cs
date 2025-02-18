using R3;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<IMobileDetectionMeshView> previousDetectedMeshViews = new();
        private readonly Material detectedMaterial;
        private readonly Material selectedMaterial;

        /// <summary>
        /// 選択されたメッシュのViewのProperty
        /// </summary>
        public readonly ReactiveProperty<IMobileDetectionMeshView> SelectedMeshViewProperty = new();

        /// <summary>
        /// MeshViewが選択されたかのObservable
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
            ClearDetected();
            ClearSelected();
        }

        /// <summary>
        /// 検出されたメッシュをクリアする
        /// </summary>
        public void ClearDetected()
        {
            var previousObjects = previousDetectedMeshViews
                .Select(previousDetectedMeshView => previousDetectedMeshView.GetGameObject())
                .Where(previousDetectedMeshViewGameObject => previousDetectedMeshViewGameObject);

            foreach (var previousObject in previousObjects)
            {
                Object.Destroy(previousObject);
            }

            previousDetectedMeshViews.Clear();
        }

        public void ClearSelected()
        {
            if (SelectedMeshViewProperty.Value == null)
            {
                return;
            }

            SelectedMeshViewProperty.Value = null;
            selectedSubject.OnNext(false);
        }

        /// <summary>
        /// 検出されたメッシュのViewを設定する
        /// </summary>
        public void SetMeshes(IReadOnlyList<IMobileDetectionMeshView> meshViews)
        {
            ClearSelected();
            ClearDetected();

            foreach (var meshView in meshViews)
            {
                previousDetectedMeshViews.Add(meshView);
            }

            foreach (var meshView in meshViews)
            {
                OnSubscribeMesh(meshView);
            }
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
            if (meshView != SelectedMeshViewProperty.Value)
            {
                return false;
            }

            meshView.MeshRenderer.material = detectedMaterial;
            SelectedMeshViewProperty.OnNext(null);
            selectedSubject.OnNext(false);
            return true;
        }

        private void OnDifferentViewSelected(IMobileDetectionMeshView meshView)
        {
            if (SelectedMeshViewProperty.Value != null)
            {
                SelectedMeshViewProperty.Value.MeshRenderer.material = detectedMaterial;
            }

            meshView.MeshRenderer.material = selectedMaterial;
            SelectedMeshViewProperty.OnNext(meshView);
            selectedSubject.OnNext(true);
        }
    }
}