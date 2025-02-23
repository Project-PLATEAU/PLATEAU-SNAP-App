using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    public class SimpleMeshModel
    {
        /// <summary>
        /// 三角形のインデックス
        /// </summary>
        public bool TryCreateFanTriangles(
            Vector3 cameraPosition,
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

            for (var vertexIndex = 1; vertexIndex < vertexCount - 1; vertexIndex++)
            {
                // 三角形の3頂点を取得
                var vertex1 = vertices[0];    // 扇形の中心
                var vertex2 = vertices[vertexIndex];    // 現在の頂点
                var vertex3 = vertices[vertexIndex + 1]; // 次の頂点

                // 三角形の法線を計算
                var normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

                // Camera から見た方向
                var cameraToTriangle = (vertex1 + vertex2 + vertex3) / 3 - cameraPosition;
                var isFacingCamera = Vector3.Dot(normal, cameraToTriangle) < 0;

                triangleList.Add(0);

                // 三角形をカメラの向きに合わせて追加
                if (isFacingCamera)
                {
                    triangleList.Add(vertexIndex);
                    triangleList.Add(vertexIndex + 1);
                }
                else
                {
                    triangleList.Add(vertexIndex + 1);
                    triangleList.Add(vertexIndex);
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