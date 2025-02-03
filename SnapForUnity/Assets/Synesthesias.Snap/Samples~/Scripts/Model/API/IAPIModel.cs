using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// APIのModel
    /// </summary>
    public interface IAPIModel
    {
        /// <summary>
        /// 画像の登録
        /// </summary>
        UniTask ImageRegisterAsync(
            Texture2D texture,
            CancellationToken cancellationToken);
    }
}