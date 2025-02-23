using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// Geospatial上に表示できるメッシュ(簡易版)のModel
    /// </summary>
    public interface IGeospatialSimpleMeshModel
    {
        /// <summary>
        /// Meshを作成する
        /// </summary>
        UniTask<GeospatialMeshResult> CreateMeshAsync(
            Camera camera,
            ISurfaceModel surface,
            Quaternion eunRotation,
            CancellationToken cancellationToken);
    }
}