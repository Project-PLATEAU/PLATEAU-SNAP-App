using Synesthesias.Snap.Runtime;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのModel(エディタ)
    /// </summary>
    public class EditorDetectionMeshModel : IDisposable
    {
        private readonly List<GameObject> anchorObjects = new();
        private readonly IGeospatialMeshModel editorGeospatialMeshModel;
        private readonly EditorDetectionMeshView meshViewTemplate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorDetectionMeshModel(
            IGeospatialMeshModel editorGeospatialMeshModel,
            EditorDetectionMeshView meshViewTemplate)
        {
            this.editorGeospatialMeshModel = editorGeospatialMeshModel;
            this.meshViewTemplate = meshViewTemplate;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
        
        /// <summary>
        /// メッシュ(surface)をAFGeospatialアンカーの位置に生成する
        /// </summary>
        public async UniTask<EditorDetectionMeshView> CreateMeshView(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken)
        {
            var meshResult = await editorGeospatialMeshModel.CreateMeshAsync(
                camera: camera,
                surface: surface,
                eunRotation: eunRotation,
                cancellationToken: cancellationToken);

            if (meshResult.ResultType != GeospatialMeshResultType.Success)
            {
                return null;
            }

            // const float DistanceFromCamera = 10.0F;

            // 画面内のランダムな位置にViewを配置する
            // var screenPosition = GetRandomScreenPosition();
            // screenPosition.z = DistanceFromCamera;
            // var worldPosition = camera.ScreenToWorldPoint(screenPosition);

            var view = Object.Instantiate(
                meshViewTemplate,
                meshResult.AnchorTransform);

            anchorObjects.Add(view.gameObject);

            view.Id = surface.GmlId;
            view.MeshFilter.mesh = meshResult.Mesh;
            view.MeshCollider.sharedMesh = meshResult.Mesh;
            view.MeshRenderer.enabled = false;

            return view;
        }

        private static Vector3 GetRandomScreenPosition()
        {
            var x = UnityEngine.Random.Range(0, Screen.width);
            var y = UnityEngine.Random.Range(0, Screen.height);

            var result = new Vector3(x, y, 0);
            return result;
        }

        /// <summary>
        /// メッシュをAFGeospatialアンカーの位置に生成する
        /// </summary>
        public EditorDetectionMeshView CreateMeshAtTransform(
            string id,
            Vector3 position,
            Quaternion rotation)
        {
            var view = Object.Instantiate(
                original: meshViewTemplate,
                position: position,
                rotation: rotation);

            view.Id = id;
            anchorObjects.Add(view.gameObject);
            return view;
        }

        /// <summary>
        /// メッシュ(surface)をAFGeospatialアンカーの位置に生成する
        /// </summary>
        // public EditorDetectionMeshView CreateMeshAtTransform(
        //     ISurfaceModel surface,
        //     Vector3 position,
        //     Quaternion rotation)
        // {
        //     var view = Object.Instantiate(meshViewTemplate, Vector3.zero, Quaternion.Euler(Vector3.zero));
        //     view.Id = surface.GmlId;
        //     var mesh = meshModel.CreateMesh(
        //         surface: surface,
        //         parent: view.GetGameObject().transform, 
        //         eunRotation: rotation);
        //     view.MeshFilter.mesh = mesh;
            
        //     view.gameObject.transform.position = position;
        //     view.gameObject.transform.rotation = rotation;
        //     anchorObjects.Add(view.gameObject);
        //     return view;
        // }



        /// <summary>
        /// 全てのメッシュを削除する
        /// </summary>
        public void Clear()
        {
            foreach (var anchorObject in anchorObjects)
            {
                Object.Destroy(anchorObject);
            }

            anchorObjects.Clear();
        }
    }
}