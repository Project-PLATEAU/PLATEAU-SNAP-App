using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    public class MeshSample : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material selectedMaterial;

        // Start is called before the first frame update
        void Start()
        {
            var vertices = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(12.20335F, 9.536743F, -4.689581F),
                new Vector3(12.20336F, 7.99001F, -4.689568F),
                new Vector3(-7.629395F, 7.990001F, 1.907349F)
            };

            if (!TryCreateFanTriangles(
                    camera: camera,
                    vertices: vertices,
                    results: out var triangles))
            {
                Debug.LogError("Failed to create triangles");
                return;
            }

            var mesh = CreateMesh(vertices, triangles);
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            
        }

        // 選択中のMeshを覚えておく
        private MeshRenderer selectedMeshRenderer;

        // Update is called once per frame
        void Update()
        {
            // 画面タップでMeshを選択
            if (Input.GetMouseButtonDown(0))
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    var meshRenderer = hit.collider.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material = selectedMaterial;

                        if (selectedMeshRenderer != null)
                        {
                            selectedMeshRenderer.material = defaultMaterial;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Camera の位置を考慮し、正しい方向で Mesh を生成
        /// </summary>
        public bool TryCreateFanTriangles(
            Camera camera,
            Vector3[] vertices,
            out int[] results)
        {
            var triangleList = new List<int>();

            if (vertices.Length < 4)
            {
                results = Array.Empty<int>();
                return false;
            }

            var vertexCount = vertices.Length;

            for (int i = 1; i < vertexCount - 1; i++)
            {
                // 三角形の3頂点を取得
                Vector3 v0 = vertices[0];    // 扇形の中心
                Vector3 v1 = vertices[i];    // 現在の頂点
                Vector3 v2 = vertices[i + 1]; // 次の頂点

                // 三角形の法線を計算
                Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

                // Camera から見た方向
                Vector3 cameraToTriangle = (v0 + v1 + v2) / 3 - camera.transform.position;
                bool isFacingCamera = Vector3.Dot(normal, cameraToTriangle) < 0;

                // 三角形をカメラの向きに合わせて追加
                triangleList.Add(0);
                if (isFacingCamera)
                {
                    triangleList.Add(i);
                    triangleList.Add(i + 1);
                }
                else
                {
                    triangleList.Add(i + 1);
                    triangleList.Add(i);
                }
            }

            results = triangleList.ToArray();
            return true;
        }

        /// <summary>
        /// Meshの生成
        /// </summary>
        public Mesh CreateMesh(Vector3[] vertices, int[] triangles)
        {
            var result = new Mesh { vertices = vertices, triangles = triangles };
            result.RecalculateNormals();
            result.RecalculateBounds();
            return result;
        }
    }
}
