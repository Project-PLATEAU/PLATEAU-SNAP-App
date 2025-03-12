using Cysharp.Threading.Tasks;
using Synesthesias.PLATEAU.Snap.Generated;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using Synesthesias.PLATEAU.Snap.Generated.Model;
using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

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
            ValidationParameterModel validationParameter,
            Texture2D texture,
            string fileName,
            CancellationToken cancellationToken)
        {
            try
            {
                Assert.IsNotNull(validationParameter, "validationParameterがnullです。");
                Assert.IsNotNull(texture, "textureがnullです。");
                Assert.IsFalse(string.IsNullOrEmpty(fileName), $"ファイル名({fileName})がnullまたは空です。");

                var pngBytesBuffer = texture.EncodeToPNG();
                using var stream = new MemoryStream(buffer: pngBytesBuffer);
                var fullFileName = $"{fileName}.png";

                var fileParameter = new FileParameter(
                    filename: fullFileName,
                    contentType: "image/png",
                    content: stream);

                var metadata = new BuildingImageMetadata(
                    gmlid: validationParameter.GmlId,
                    from: validationParameter.FromCoordinate,
                    to: validationParameter.ToCoordinate,
                    roll: validationParameter.Roll,
                    timestamp: validationParameter.Timestamp);

                var metaDataJson = metadata.ToJson();

                var response = await imagesApiAsync.CreateBuildingImageAsyncAsync(
                    file: fileParameter,
                    metadata: metaDataJson,
                    cancellationToken: cancellationToken);

                response.ThrowIfError();
            }
            catch (ApiException exception)
            {
                Debug.LogWarning(exception);
                throw;
            }
            catch (BuildingImageException exception)
            {
                Debug.LogWarning(exception);
                throw;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
                throw;
            }
        }
    }
}