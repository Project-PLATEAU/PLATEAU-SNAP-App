using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 撮影判定画面のModel
    /// </summary>
    public class ValidationModel : IValidationModel, IDisposable
    {
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public UniTask StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キャプチャしたテクスチャを取得
        /// </summary>
        public Texture GetCapturedTexture()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 登録
        /// </summary>
        public UniTask RegisterAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}