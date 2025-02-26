using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// TriangulationのModel
    /// </summary>
    public interface ITriangulationModel
    {
        /// <summary>
        /// 三角形の配列を取得する
        /// </summary>
        public int[] GetTriangles(Vector3[] hullVertices);

        /// <summary>
        /// メッシュを取得する
        /// </summary>
        public Mesh GetMesh(Camera camera, Vector3[] hullVertices, Vector3[][] holesVertices);
    }
}