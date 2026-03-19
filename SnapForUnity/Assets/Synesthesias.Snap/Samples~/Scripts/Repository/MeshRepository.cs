using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

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
        private readonly DetectionMaterialModel materialModel;

        /// <summary>
        /// 選択されたメッシュのViewのProperty
        /// </summary>
        public readonly ReactiveProperty<IMobileDetectionMeshView> SelectedMeshViewProperty = new();

        /// <summary>
        /// 検出されたメッシュのViewのリスト
        /// </summary>
        public IReadOnlyCollection<IMobileDetectionMeshView> DetectedMeshViews
            => detectedMeshViews.Values.ToArray();

        /// <summary>
        /// MeshViewが選択されたかのObservable
        /// </summary>
        public Observable<bool> OnSelectedAsObservable()
            => selectedSubject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeshRepository(DetectionMaterialModel materialModel)
        {
            this.materialModel = materialModel;
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
            // コレクション変更エラーを避けるため、配列にコピーしてから削除
            var meshViewsToRemove = detectedMeshViews.Values.ToArray();
            foreach (var meshView in meshViewsToRemove)
            {
                if (meshView?.GetGameObject())
                {
                    Object.Destroy(meshView.GetGameObject());
                }
            }
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
                // 重複したメッシュの場合は既存のメッシュを保持し、新しいメッシュは破棄する
                // Debug.Log($"メッシュ({meshView.Id})は既に検出されています。新しいメッシュを破棄します。");
                if (meshView?.GetGameObject())
                {
                    Object.Destroy(meshView.GetGameObject());
                }
                return;
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
            // コレクション変更エラーを避けるため、配列にコピーしてから削除
            var meshViewsArray = meshViews.ToArray();
            foreach (var meshView in meshViewsArray)
            {
                RemoveDetected(meshView.Id);
            }
        }

        private void OnSubscribeMesh(IMobileDetectionMeshView meshView)
        {
            if (meshView == null)
            {
                return;
            }

            GameObject gameObject = null;
            try
            {
                gameObject = meshView.GetGameObject();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[購読設定] GameObject取得に失敗: {ex.Message}");
                return;
            }

            if (!gameObject)
            {
                return;
            }

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

            meshView.MeshRenderer.material = materialModel.DetectedMaterial;
            SelectedMeshViewProperty.OnNext(null);
            selectedSubject.OnNext(false);
            return true;
        }

        private void OnDifferentViewSelected(IMobileDetectionMeshView meshView)
        {
            if (SelectedMeshViewProperty.Value?.MeshRenderer)
            {
                SelectedMeshViewProperty.Value.MeshRenderer.material = materialModel.DetectedMaterial;
            }

            meshView.MeshRenderer.material = materialModel.SelectedMaterial;
            SelectedMeshViewProperty.OnNext(meshView);
            selectedSubject.OnNext(true);
        }
    }
}