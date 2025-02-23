using Synesthesias.Snap.Runtime;
using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
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
        private readonly EditorDetectionMeshView meshViewTemplate;
        private double originLatitude;
        private double originLongitude;
        private double originAltitude;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorDetectionMeshModel(EditorDetectionMeshView meshViewTemplate)
        {
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
        public EditorDetectionMeshView CreateMeshAtARGeospatialAnchor(
            double latitude,
            double longitude,
            double altitude, 
            ISurfaceModel detectedSurface)
        {
            var gmlId = detectedSurface.GmlId;
            var model = new MeshModel();

            // メッシュを生成する緯度経度の原点を設定する
            if (originLatitude == 0 && originLongitude == 0)
            {
                originLatitude = detectedSurface.Coordinates[0][0][1];
                originLongitude = detectedSurface.Coordinates[0][0][0];
                (originLatitude, originLongitude, originAltitude) = ShapeCalculator.GetMeshCenter(detectedSurface.Coordinates);
            }

            var view = Object.Instantiate(meshViewTemplate, Vector3.zero, Quaternion.Euler(Vector3.zero));
            Debug.Log($"GML ID: {gmlId}");
            var mesh = model.CreateMesh(detectedSurface, gmlId, originLatitude, originLongitude, originAltitude);
            view.MeshFilter.mesh = mesh;
            model.CreateCollider(gmlId, mesh, view.transform);

            anchorObjects.Add(view.gameObject);

            return view;
        }

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