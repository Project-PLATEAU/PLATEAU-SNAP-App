using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// モックサーバーのModel
    /// Mac/PCでローカルサーバーを立てて画像送信の動作検証をするためのModel
    /// </summary>
    public class MockServerModel : IServerModel, IDisposable
    {
        private readonly HttpListener listener;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MockServerModel(IEndPointModel endPointModel)
        {
            listener = new HttpListener();
            var endPoint = endPointModel.GetEndPoint();
            listener.Prefixes.Add(uriPrefix: endPoint);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }

        /// <summary>
        /// サーバーを起動する
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationTokenSource != null)
            {
                throw new InvalidOperationException("既に起動しています");
            }

            cancellationTokenSource = new CancellationTokenSource();

            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationTokenSource.Token,
                cancellationToken);

            using var listener = this.listener;

            try
            {
                listener.Start();
                Debug.Log("MockServer: 起動しました");

                while (true)
                {
                    var context = await this.listener
                        .GetContextAsync()
                        .AsUniTask()
                        .AttachExternalCancellation(tokenSource.Token);

                    await HandleRequestAsync(context, tokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("MockServer: キャンセルされました");
                throw;
            }
            finally
            {
                listener.Stop();
                listener.Close();
                ((IDisposable)listener)?.Dispose();
                cancellationTokenSource = null;
                Debug.Log("MockServer: 停止しました");
            }
        }

        static async UniTask HandleRequestAsync(
            HttpListenerContext context,
            CancellationToken cancellationToken)
        {
            var request = context.Request;
            var response = context.Response;

            switch (request.Url.AbsolutePath)
            {
                case "/texture/register":
                    await RegisterTextureAsync(request, response, cancellationToken);
                    break;
                default:
                    response.StatusCode = (int)ServerStatusCode.NotFound;
                    break;
            }

            response.OutputStream.Close();
        }

        static async UniTask RegisterTextureAsync(
            HttpListenerRequest request,
            HttpListenerResponse response,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    response.StatusCode = (int)ServerStatusCode.MethodNotAllowed;
                    return;
                }

                if (!request.HasEntityBody)
                {
                    response.StatusCode = (int)ServerStatusCode.BadRequest;
                    return;
                }

                var saveFileName = $"texture_{DateTime.Now:yyyyMMddHHmmss}.png";
                var saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
                await using var streamBody = request.InputStream;
                await using var fileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
                await streamBody.CopyToAsync(fileStream, cancellationToken);

                var responseBytes = Encoding.UTF8.GetBytes("Success");

                await response.OutputStream.WriteAsync(
                    buffer: responseBytes,
                    offset: 0,
                    count: responseBytes.Length,
                    cancellationToken: cancellationToken);

                // response.Close();

                // // リクエストから画像データを受信する
                // var requestStream = request.InputStream;
                // var requestBuffer = new byte[request.ContentLength64];
                // await requestStream.ReadAsync(
                //     buffer: requestBuffer,
                //     offset: 0,
                //     count: requestBuffer.Length,
                //     cancellationToken: cancellationToken);
                //
                // // 画像データを保存する
                // var saveFileName = $"texture_{DateTime.Now:yyyyMMddHHmmss}.png";
                //
                //
                // var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseJson);
                // response.ContentLength64 = responseBuffer.Length;
                // response.StatusCode = (int)ServerStatusCode.Success;
                //
                //
                // var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                // var saveFileName = $"texture_{dateTime}.png";
                // var savePath = Path.Combine(Application.persistentDataPath, saveFileName);
                //
                // await response.OutputStream.WriteAsync(
                //     buffer: responseBuffer,
                //     offset: 0,
                //     count: responseBuffer.Length,
                //     cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                response.StatusCode = (int)ServerStatusCode.InternalServerError;
                response.StatusDescription = e.Message;
            }
        }
    }
}