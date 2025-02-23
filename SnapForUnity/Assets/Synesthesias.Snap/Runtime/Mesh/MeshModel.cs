using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュのModel
    /// </summary>
    public class MeshModel
    {
        private List<Vector3> originalHullVertices;
        private Shape plainShapeData;
        public List<Vector3> OriginalHullVertices { get { return originalHullVertices; } }
        private List<List<Vector3>> originalHolesVertices;
        public List<List<Vector3>> OriginalHolesVertices { get { return originalHolesVertices; } }
        private List<Vector3> rotatedHullVertices;
        public List<Vector3> RotatedHullVertices { get { return rotatedHullVertices; } }
        private List<List<Vector3>> rotatedHolesVertices;
        public List<List<Vector3>> RotatedHolesVertices { get { return rotatedHolesVertices; } }
        private string gmlId;
        public string GmlId { get { return gmlId; } }

        private float rotationAxisY = 0;
        private const float debugSphereRadius = 0.5f;
        private bool isCheckedVertices = false;

        /// <summary>
        /// メッシュの生成
        /// </summary>
        public Mesh CreateMesh(ISurfaceModel surface, string id, double originLatitude, double originLongitude, double originAltitude)
        {
            // Trianglurationの設定
            Application.targetFrameRate = 60;
            var mesh = new Mesh();
            mesh.MarkDynamic();

            // 当たり判定用のColliderを作成, gmlIdの設定
            gmlId = id;

            originalHullVertices = new List<Vector3>();
            originalHolesVertices = new List<List<Vector3>>();

            // 地理情報を取得
            var latLonHullData = surface.Coordinates[0].ToArray().Take(surface.Coordinates[0].Count - 1).ToList();
            var latLonHolesData = new List<List<List<double>>>();
            if (surface.Coordinates.Count > 1)
            {
                foreach (var coordinate in surface.Coordinates)
                {
                    latLonHolesData.Add(coordinate.ToArray().Take(coordinate.Count - 1).ToList());
                }
            }

            // 緯度軽度高度->メートルに変換(x:緯度, y:高度, z:経度)
            for (int index = 0; index < latLonHullData.Count; index++)
            {
                originalHullVertices.Add(LatLonConverter.ToMeters(                    
                    latitude: latLonHullData[index][1],
                    longitude: latLonHullData[index][0], 
                    altitude: latLonHullData[index][2], 
                    originLatitude: originLatitude, 
                    originLongitude: originLongitude,
                    originAltitude: originAltitude));
            }

            if (latLonHolesData != null)
            {
                foreach (var holeData in latLonHolesData)
                {
                    var holeVertices = new List<Vector3>();
                    for (int index = 0; index < holeData.Count; index++)
                    {
                        holeVertices.Add(LatLonConverter.ToMeters(
                            latitude: holeData[index][1], 
                            longitude: holeData[index][0], 
                            altitude: holeData[index][2],
                            originLatitude: originLatitude,
                            originLongitude: originLongitude,
                            originAltitude: originAltitude));
                    }
                    originalHolesVertices.Add(holeVertices);
                }
            }

            // メッシュを生成する用の頂点座標を設定
            rotationAxisY = ShapeCalculator.GetRotationAxisY(originalHullVertices);
            rotatedHullVertices = ShapeCalculator.GetRotatedVertices(originalHullVertices, rotationAxisY);

            rotatedHolesVertices = new List<List<Vector3>>();
            foreach (var holeVertices in originalHolesVertices)
            {
                rotatedHolesVertices.Add(ShapeCalculator.GetRotatedVertices(holeVertices, rotationAxisY));
            }

            // x,y,z座標の内、x,yを使用してメッシュを生成
            plainShapeData = new Shape
            {
                hull = ShapeCalculator.GetHullVertices2d(rotatedHullVertices),
                holes = ShapeCalculator.GetHolesVertices2d(rotatedHolesVertices)
            };

            // メッシュを生成
            ShapeCalculator.GeneratePlainShape(plainShapeData, mesh);

            // メッシュを生成した時にZ成分が失われるので，これを元に戻す
            float offsetZ = rotatedHullVertices[0].z;
            mesh.vertices = ShapeCalculator.RestoredOffsetZ(mesh.vertices, offsetZ);

            // verticesに渡す頂点を作成
            var invertRotaionMatrix = ShapeCalculator.GetInvertRotationMatrix(rotationAxisY);
            var restoredVertices = ShapeCalculator.GetRestoredVertices(mesh.vertices.ToList(), invertRotaionMatrix);

            // 頂点を渡す
            mesh.vertices = restoredVertices.ToArray();
            
            return mesh;
        }

        /// <summary>
        /// Colliderの生成
        /// </summary>
        public Collider CreateCollider(
            string id,
            Mesh mesh,
            Transform parent)
        {
            GameObject colliderObject = new GameObject(id);
            var bounds = mesh.bounds;
            
            colliderObject.transform.position = ShapeCalculator.GetMeshCenter(mesh.vertices);
            colliderObject.transform.rotation = Quaternion.Euler(0, -rotationAxisY, 0);
            colliderObject.transform.localScale = new Vector3(bounds.size.x, bounds.size.y, 0.5f);
            colliderObject.transform.SetParent(parent);
            var collider = colliderObject.AddComponent<BoxCollider>();
            return collider;
        }

        /// <summary>
        /// 頂点が正しい位置に配置されているか確認する用
        /// </summary>
        /// <param name="material">表示させる頂点の色</param>
        public void CheckVertices(Material material)
        {
            foreach (Vector3 vertex in originalHullVertices)
            {
                //Debug.Log(vertex);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                sphere.transform.position = new Vector3(vertex.x, vertex.y, vertex.z);

                sphere.transform.localScale = Vector3.one * debugSphereRadius;

                Renderer renderer = sphere.GetComponent<Renderer>();
                renderer.material = material;
            }
        }
    }
}

