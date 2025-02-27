using R3;
using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, IMobileDetectionMeshView> detectedMeshViews = new();
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
        /// 検出されたメッシュを削除する
        /// </summary>
        public void RemoveDetected(string id)
        {
            if (!detectedMeshViews.TryGetValue(id, out var meshView))
            {
                return;
            }

            Object.Destroy(meshView.GetGameObject());
            detectedMeshViews.Remove(id);
        }

        public void ClearDetected()
        {
            RemoveDetected(detectedMeshViews.Values);
            detectedMeshViews.Clear();
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
        /// メッシュIDを含むか
        /// </summary>
        public bool ContainsMeshId(string id)
        {
            return detectedMeshViews.ContainsKey(id);
        }

        /// <summary>
        /// 検出されたメッシュのViewを設定する
        /// </summary>
        public void SetMesh(IMobileDetectionMeshView meshView)
        {
            if (ContainsMeshId(meshView.Id))
            {
                throw new InvalidOperationException($"メッシュ({meshView.Id})は既に検出されています");
            }

            RemoveDetected(meshView.Id);
            detectedMeshViews[meshView.Id] = meshView;
            OnSubscribeMesh(meshView);
        }

        /// <summary>
        /// メッシュのGameObjectを選択する
        /// </summary>
        public void SelectObject(GameObject gameObject)
        {
            selectedObjectSubject.OnNext(gameObject);
        }

        private void RemoveDetected(IEnumerable<IMobileDetectionMeshView> meshViews)
        {
            foreach (var meshView in meshViews)
            {
                RemoveDetected(meshView.Id);
            }
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
            if (SelectedMeshViewProperty.Value?.MeshRenderer)
            {
                SelectedMeshViewProperty.Value.MeshRenderer.material = detectedMaterial;
            }

            meshView.MeshRenderer.material = selectedMaterial;
            SelectedMeshViewProperty.OnNext(meshView);
            selectedSubject.OnNext(true);
        }
    }
}