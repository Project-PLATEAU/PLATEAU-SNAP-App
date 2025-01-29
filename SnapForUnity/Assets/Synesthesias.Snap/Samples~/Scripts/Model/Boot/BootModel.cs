using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// BootシーンのModel
    /// </summary>
    public class BootModel : IAsyncStartable, IDisposable
    {
        private readonly TMPModel tmpModel;
        private readonly LocalizationModel localizationModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BootModel(LocalizationModel localizationModel)
        {
            this.localizationModel = localizationModel;

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
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await UniTask.WhenAll(
                tmpModel.LoadAsync(cancellationToken),
                localizationModel.StartAsync(cancellationToken));
        }
    }
}