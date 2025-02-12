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
        public Mesh CreateMesh(int latLonDataIndex, string id)
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
            var latLonHullData = LatLonTests.TestData[latLonDataIndex].hull;
            var latLonHolesData = LatLonTests.TestData[latLonDataIndex].holes;

            // 緯度軽度高度->メートルに変換(x:緯度, y:高度, z:経度)
            for (int index = 0; index < latLonHullData.GetLength(0); index++)
            {
                originalHullVertices.Add(LatLonConverter.ToMeters(latLonHullData[index][0], latLonHullData[index][1], latLonHullData[index][2]));
            }

            if (latLonHolesData != null)
            {
                foreach (var holeData in latLonHolesData)
                {
                    var holeVertices = new List<Vector3>();
                    for (int index = 0; index < holeData.GetLength(0); index++)
                    {
                        holeVertices.Add(LatLonConverter.ToMeters(holeData[index][0], holeData[index][1], holeData[index][2]));
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
    }
}

