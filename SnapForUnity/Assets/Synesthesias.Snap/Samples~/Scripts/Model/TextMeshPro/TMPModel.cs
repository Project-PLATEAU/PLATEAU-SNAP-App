using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine.AddressableAssets;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// TextMeshProのハンドラ
    /// </summary>
    public class TMPModel : IDisposable
    {
        private readonly string path;
        private TMP_FontAsset fontAsset;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TMPModel(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (!fontAsset)
            {
                return;
            }

            fontAsset.ClearFontAssetData();
            Addressables.Release(fontAsset);
        }

        /// <summary>
        /// 読込
        /// </summary>
        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            fontAsset = await Addressables.LoadAssetAsync<TMP_FontAsset>(path)
                .WithCancellation(cancellationToken);

            fontAsset.ClearFontAssetData();
        }
    }
}