using Cysharp.Threading.Tasks;
using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.PLATEAU.Snap.Generated.Client;
using Synesthesias.PLATEAU.Snap.Generated.Model;
using Synesthesias.Snap.Runtime;
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
            ValidationParameterModel validationParameter,
            Texture2D texture,
            string fileName,
            CancellationToken cancellationToken)
        {
            try
            {
                // 各種引数のNullをDebug.Logで出力する
                Debug.Log($"validationParameter(null?): {validationParameter == null}");
                Debug.Log($"texture(null?): {texture == null}");
                Debug.Log($"fileName(null?): {fileName == null}");

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

                await imagesApiAsync.CreateBuildingImageAsyncAsync(
                    file: fileParameter,
                    metadata: metaDataJson,
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