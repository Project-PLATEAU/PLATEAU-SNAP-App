using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// APIモデル(モック)
    /// MockServerと一緒に使用する
    /// </summary>
    public class MockAPIModel : IAPIModel
    {
        private readonly IEndPointModel endPointModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MockAPIModel(
            IEndPointModel endPointModel)
        {
            this.endPointModel = endPointModel;
        }

        /// <summary>
        /// 画像の登録
        /// </summary>
        public async UniTask ImageRegisterAsync(
            Texture2D texture,
            CancellationToken cancellationToken)
        {
            var endPoint = endPointModel.GetEndPoint();
            var url = $"{endPoint}texture/register";
            var request = new UnityWebRequest(url: url, method: "POST");
            var bytes = texture.EncodeToPNG();
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.SetRequestHeader("Content-Type", "image/png");

            // モックなので通信はしない
            await UniTask.WaitForSeconds(3, cancellationToken: cancellationToken);
        }
    }
}