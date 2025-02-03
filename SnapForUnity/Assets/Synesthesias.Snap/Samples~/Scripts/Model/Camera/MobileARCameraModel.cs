using UnityEngine;
using UnityEngine.UI;

namespace Synesthesias.Snap.Sample
{
    public class MobileARCameraModel
    {
        private readonly RawImage rawImage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MobileARCameraModel(RawImage rawImage)
        {
            this.rawImage = rawImage;
        }

        public bool TryCaptureTexture2D(out Texture2D result)
        {
            try
            {
                var originalTexture = rawImage.texture as RenderTexture;

                var copiedTexture = new Texture2D(
                    width: originalTexture.width,
                    height: originalTexture.height,
                    textureFormat: TextureFormat.RGBA32,
                    mipChain: false,
                    linear: true);

                var previousTexture = RenderTexture.active;
                RenderTexture.active = originalTexture;

                copiedTexture.ReadPixels(
                    source: new Rect(0, 0, originalTexture.width, originalTexture.height),
                    destX: 0,
                    destY: 0);

                copiedTexture.Apply();
                RenderTexture.active = previousTexture;
                result = copiedTexture;
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}