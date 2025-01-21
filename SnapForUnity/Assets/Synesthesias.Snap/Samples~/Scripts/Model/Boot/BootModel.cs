using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// BootシーンのModel
    /// </summary>
    public class BootModel : IDisposable
    {
        private readonly TMPModel tmpModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BootModel()
        {
            tmpModel =
                new TMPModel(
                    "Assets/Samples/Synesthesias.Snap/Addressables/Text/NotoSansJP-Regular/TextMeshPro/NotoSansJP_dynamic.asset");
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            tmpModel.Dispose();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            await tmpModel.LoadAsync(cancellationToken);
        }
    }
}