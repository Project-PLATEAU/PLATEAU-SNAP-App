using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 撮影判定画面のModel
    /// </summary>
    public interface IValidationModel : IAsyncStartable
    {
        /// <summary>
        /// キャプチャしたテクスチャを取得
        /// </summary>
        Texture GetCapturedTexture();

        /// <summary>
        /// 登録
        /// </summary>
        UniTask RegisterAsync(CancellationToken cancellationToken);

        /// <summary>
        /// キャンセル
        /// </summary>
        void Cancel();
    }
}