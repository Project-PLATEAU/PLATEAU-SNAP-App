using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Synesthesias.Snap.Runtime
{
    public class VectorCalculatorModel
    {
        public Vector3 RotateByMatrix(Vector3 vertex, Matrix4x4 rotationMatrix)
        {
            // 回転行列を適用
            var rotatedVertex = rotationMatrix.MultiplyPoint3x4(vertex);

            return rotatedVertex;
        }

        public Matrix4x4 GetInvertRotationMatrix(float angle)
        {
            // 回転軸と角度を指定（例: Y軸周りに45度回転）
            var rotationAxis = Vector3.up;

            // 回転行列を作成
            var rotationQuaternion = Quaternion.AngleAxis(angle, rotationAxis);
            var rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);

            // 回転行列の逆行列を計算
            var inverseRotationMatrix = rotationMatrix.inverse;

            return inverseRotationMatrix;
        }

        public Vector3[] GetRestoredVertices(Vector3[] vertices, Matrix4x4 inverseMatrix)
        {
            var results = vertices
                .Select(vertex => RotateByMatrix(vertex, inverseMatrix))
                .ToArray();

            return results;
        }

        public Vector3[] RestoredOffsetZ(Vector3[] vertices, float offsetZ)
        {
            var results = vertices
                .Select(vertex => new Vector3(vertex.x, vertex.y, offsetZ))
                .ToArray();

            return results;
        }

        public Vector2[] GetHullVertices2d(Vector3[] hullVertices)
        {
            var results = hullVertices
                .Select(hullVertex => new Vector2(hullVertex.x, hullVertex.y))
                .ToArray();

            return results;
        }

        public Vector2[][] GetHolesVertices2d(Vector3[][] holesVertices)
        {
            if (holesVertices == null || holesVertices.Length == 0)
            {
                return Array.Empty<Vector2[]>();
            }

            var results = holesVertices
                .Select(holeVertices =>
                {
                    if (holeVertices == null || holeVertices.Length == 0)
                    {
                        return Array.Empty<Vector2>();
                    }

                    return holeVertices
                        .Select(vertex => new Vector2(vertex.x, vertex.y))
                        .ToArray();
                }).ToArray();

            return results;
        }

        public float GetRotationAxisY(Vector3[] vertices)
        {
            // verticesの法線ベクトル
            var normal = NormalVectorFrom3d(vertices);

            var angle =  Vector3.Angle(normal,Vector3.up);

            if (Math.Abs(angle) <= 2 || Math.Abs(angle) >= 178)
            {
                return float.NaN;
            }

            var normalXY = new Vector3(0, 1, 0);
            var normalXZ = new Vector3(0, 0, -1);
            // normalをXZ平面に投影
            var fromNormal = Vector3.ProjectOnPlane(normal, normalXY);
            // もう一方のベクトルをnormalXZに設定
            var toNormal = normalXZ;
            var rotationAngle = Vector3.SignedAngle(fromNormal, toNormal, normalXY);

            return rotationAngle;
        }

        public Vector3[] GetRotatedVertices(Vector3[] vertices, float rotationAngle)
        {
            var results = vertices.Select(vertex =>
            {
                // 回転行列を作成(XZ平面)
                var rotationQuaternion = Quaternion.Euler(0, rotationAngle, 0);
                var rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);
                return RotateByMatrix(vertex, rotationMatrix);
            }).ToArray();

            return results;
        }

        // メッシュの中心座標を取得
        public Vector3 GetMeshCenter(Vector3[] vertices)
        {
            var sum = Vector3.zero;

            if (vertices.Length == 0) return sum;

            foreach (Vector3 vertex in vertices)
            {
                sum += vertex;
            }

            return sum / vertices.Length;
        }

        // メッシュの中心座標を取得
        public (double latitude, double longitude, double altitude) GetMeshCenter(
            List<List<double>> coordinateList)
        {
            double latitudeSum = 0;
            double longitudeSum = 0;
            double altitudeSum = 0;

            foreach (var coordinate in coordinateList)
            {
                latitudeSum += coordinate[1];
                longitudeSum += coordinate[0];
                altitudeSum += coordinate[2];
            }

            return (latitudeSum / coordinateList.Count, longitudeSum / coordinateList.Count,
                altitudeSum / coordinateList.Count);
        }

        // 検出可能面の法線ベクトルを求める関数
        public Vector3 NormalVectorFrom3d(Vector3[] vertices)
        {
            var n = vertices.Length;
            var normal = Vector3.zero;

            for (var i = 0; i < n; i++)
            {
                var current = vertices[i];
                var next = vertices[(i + 1) % n];

                normal += Vector3.Cross(current, next);
            }

            return normal.normalized;
        }

        public Vector3 NormalVectorFrom2d(Vector2[] vertices)
        {
            var n = vertices.Length;
            var normal = Vector3.zero;

            for (var i = 0; i < n; i++)
            {
                var current = vertices[i];
                var next = vertices[(i + 1) % n];

                normal += Vector3.Cross(current, next);
            }

            return normal.normalized;
        }

        /// <summary>
        /// メッシュを反転する
        /// TODO: メッシュ生成後に反転するのは非効率なので，メッシュ生成時に反転するように変更する
        /// </summary>
        [Obsolete("削除予定")]
        public Mesh GetInvertMesh(Mesh mesh)
        {
            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            mesh.triangles = triangles;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}