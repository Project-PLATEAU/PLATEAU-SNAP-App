using Cysharp.Threading.Tasks;
using System.Threading;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// サーバーのModel
    /// </summary>
    public class ServerModel : IServerModel
    {
        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // 本番ではサーバーは起動しない
            await UniTask.Yield();
        }
    }
}