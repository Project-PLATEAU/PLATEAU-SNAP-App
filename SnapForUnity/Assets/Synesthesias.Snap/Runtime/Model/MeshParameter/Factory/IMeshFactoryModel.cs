using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュ生成用のModel
    /// </summary>
    public interface IMeshFactoryModel
    {
        /// <summary>
        /// メッシュを生成する
        /// </summary>
        UniTask<Mesh> CreateAsync(
            Vector3[] hull,
            Vector3[][] holes,
            CancellationToken cancellationToken);
    }
}