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
        UniTask<int[]> GetTriangles(
            Vector3[] hullVertices,
            CancellationToken cancellationToken);

        /// <summary>
        /// メッシュを取得する
        /// </summary>
        UniTask<Mesh> CreateMeshAsync(
            Camera camera,
            Vector3[] hullVertices,
            Vector3[][] holesVertices,
            CancellationToken cancellationToken);
    }
}