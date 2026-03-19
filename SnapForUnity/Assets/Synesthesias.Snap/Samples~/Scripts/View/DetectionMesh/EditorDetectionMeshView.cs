using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.Union;
    
namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のメッシュのView(エディタ用)
    /// </summary>
    public class EditorDetectionMeshView : MonoBehaviour, IMobileDetectionMeshView
    {
        [SerializeField] private string id;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private Material debugSphereMaterial;

        /// <summary>
        /// メッシュのID
        /// </summary>
        public string Id
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// メッシュのFilter
        /// </summary>
        public MeshFilter MeshFilter
            => meshFilter;

        /// <summary>
        /// メッシュのRenderer
        /// </summary>
        public MeshRenderer MeshRenderer
            => meshRenderer;

        public Material DebugSphereMaterial
            => debugSphereMaterial;

        /// <summary>
        /// GameObjectを取得する
        /// </summary>
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// メッシュのCollider
        /// </summary>
        public MeshCollider MeshCollider
            => meshCollider;

        // <summary>
        /// 子GameObjectを取得する
        /// </summary>
        public GameObject GetChildGameObject()
        {
            return gameObject.transform.GetChild(0).gameObject;
        }

         /// <summary>
        /// メッシュの頂点をWKT形式のPolygon文字列で取得する
        /// </summary>
        /// <param name="camera">変換に使用するカメラ</param>
        /// <returns>WKT形式のPolygon文字列</returns>
        public string GetVerticesAsScreenCoordinates(Camera camera)
        {
            var polygons = CreateTrianglePolygons(camera);  
            
            // ポリゴンを統合
            var unionedPolygon = CascadedPolygonUnion.Union(polygons.Cast<Geometry>().ToArray());
            
            // WKT形式の文字列を生成    
            var writer = new WKTWriter();
            return writer.Write(unionedPolygon);
        }

        /// <summary>
        /// 三角形のポリゴンを作成する
        /// </summary>
        /// <param name="camera">変換に使用するカメラ</param>
        /// <returns>三角形のポリゴン</returns>
        private IEnumerable<Polygon> CreateTrianglePolygons(Camera camera)
        {
            var mesh = meshFilter.sharedMesh;
            var meshTransform = meshFilter.transform;
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var geometryFactory = new GeometryFactory();
            
            for (int i = 0; i < triangles.Length; i += 3)
            {
                var polygon = CreateTrianglePolygon(
                    vertices[triangles[i]],
                    vertices[triangles[i + 1]],
                    vertices[triangles[i + 2]],
                    meshTransform,
                    camera,
                    geometryFactory
                );
                
                yield return polygon;
            }
        }

        /// <summary>
        /// 三角形のポリゴンを作成する
        /// </summary>
        /// <param name="v1">三角形の頂点1</param>
        /// <param name="v2">三角形の頂点2</param>
        /// <param name="v3">三角形の頂点3</param>  
        /// <param name="meshTransform">メッシュのTransform</param>
        /// <param name="camera">変換に使用するカメラ</param>
        /// <param name="geometryFactory">GeometryFactory</param>
        /// <returns>三角形のポリゴン</returns>
        private Polygon CreateTrianglePolygon(Vector3 v1, Vector3 v2, Vector3 v3, Transform meshTransform, Camera camera, GeometryFactory geometryFactory){
            // 3つの頂点をスクリーン座標に変換
            var screen1 = ConvertToScreenCoordinate(v1, meshTransform, camera);
            var screen2 = ConvertToScreenCoordinate(v2, meshTransform, camera);
            var screen3 = ConvertToScreenCoordinate(v3, meshTransform, camera);
            
            // ポリゴンを作成
            var coordinates = new[]
            {
                new Coordinate(screen1.x, screen1.y),
                new Coordinate(screen2.x, screen2.y),
                new Coordinate(screen3.x, screen3.y),
                new Coordinate(screen1.x, screen1.y)
            };
            
            return geometryFactory.CreatePolygon(coordinates);
        }

        /// <summary>
        /// ローカル座標をスクリーン座標に変換する
        /// </summary>
        /// <param name="localVertex">ローカル座標</param>
        /// <param name="meshTransform">メッシュのTransform</param>
        /// <param name="camera">変換に使用するカメラ</param>
        /// <returns>スクリーン座標</returns>   
        private Vector3 ConvertToScreenCoordinate(Vector3 localVertex, Transform meshTransform, Camera camera){
            // ローカル→スクリーン座標変換
            var worldVertex = meshTransform.TransformPoint(localVertex);
            var screenPoint = camera.WorldToScreenPoint(worldVertex);
            
            // Y座標を反転して左下を(0,0)にする
            return new Vector3(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
        }
    }
}