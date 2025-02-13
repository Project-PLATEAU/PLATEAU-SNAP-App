using Cysharp.Threading.Tasks;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 撮影した建物面の画像を登録するリポジトリ
    /// </summary>
    public class ImageRepository
    {
        private readonly IImagesApiAsync imagesApiAsync;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageRepository(IImagesApiAsync imagesApiAsync)
        {
            this.imagesApiAsync = imagesApiAsync;
        }

        /// <summary>
        /// 建物画像を作成する
        /// </summary>
        public async UniTask CreateBuildingImageAsyncAsync(
            Texture2D texture,
            string fileName,
            CancellationToken cancellationToken)
        {
            try
            {
                var pngBytesBuffer = texture.EncodeToPNG();
                using var stream = new MemoryStream(buffer: pngBytesBuffer);
                var fullFileName = $"{fileName}.png";

                var fileParameter = new FileParameter(
                    filename: fullFileName,
                    contentType: "image/png",
                    content: stream);

                // TODO: metadataを設定する
                var metadata = string.Empty;

                await imagesApiAsync.CreateBuildingImageAsyncAsync(
                    file: fileParameter,
                    metadata: metadata,
                    cancellationToken: cancellationToken);
            }
            catch (ApiException exception)
            {
                Debug.LogError(exception);
                throw;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                throw;
            }
        }
    }
}