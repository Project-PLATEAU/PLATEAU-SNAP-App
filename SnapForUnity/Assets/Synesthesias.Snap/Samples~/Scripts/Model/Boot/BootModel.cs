using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// BootシーンのModel
    /// </summary>
    public class BootModel : IAsyncStartable
    {
        private readonly LocalizationModel localizationModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BootModel(LocalizationModel localizationModel)
        {
            this.localizationModel = localizationModel;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await UniTask.WhenAll(
                localizationModel.StartAsync(cancellationToken));
        }
    }
}