using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synesthesias.Snap.Sample
{
    public class EditorWebCameraModel : IDisposable
    {
        private readonly WebCamDevice[] devices;
        private readonly WebCamTexture webCamTexture;
        private readonly RenderTexture renderTexture;
        private int deviceIndex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorWebCameraModel(RenderTexture renderTexture)
        {
            this.renderTexture = renderTexture;
            webCamTexture = new WebCamTexture();
            devices = WebCamTexture.devices;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            webCamTexture.Stop();

            if (!webCamTexture)
            {
                return;
            }

            Object.Destroy(webCamTexture);
        }

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            SetDeviceName(webCamTexture.deviceName);
            webCamTexture.Play();
            await UniTask.Yield();
        }

        public Texture GetCameraTexture()
        {
            return webCamTexture;
        }

        /// <summary>
        /// カメラデバイスの切り替え
        /// </summary>
        public void ToggleDevice()
        {
            SetDeviceIndex(deviceIndex + 1);
        }

        public bool TryCaptureTexture2D(out Texture2D result)
        {
            try
            {
                result = new Texture2D(
                    webCamTexture.width,
                    webCamTexture.height,
                    textureFormat: TextureFormat.RGBA32,
                    mipChain: false,
                    linear: false);

                var colors = webCamTexture.GetPixels();
                result.SetPixels(colors);
                result.Apply();

                // TODO: RenderTextureの画像をresultに乗算する
            }
            catch
            {
                result = null;
                return false;
            }

            return true;
        }

        private void SetDeviceName(string deviceName)
        {
            var result = devices
                .Select((device, index) => (device, index))
                .FirstOrDefault(tuple => tuple.device.name == deviceName);

            deviceIndex = result.index;
        }

        private void SetDeviceIndex(int index)
        {
            var clampedIndex = index % devices.Length;

            if (deviceIndex == clampedIndex)
            {
                return;
            }

            deviceIndex = clampedIndex;
            webCamTexture.Stop();
            var device = devices[deviceIndex];
            webCamTexture.deviceName = device.name;
            webCamTexture.Play();
        }
    }
}