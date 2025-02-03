using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// Screen関連のModel
    /// </summary>
    public class ScreenModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector2 GetAdjustedSizeDelta(int width, int height)
        {
            var screenRatio = (float)Screen.width / Screen.height;
            var imageRatio = (float)width / height;

            var result = imageRatio > screenRatio
                ? new Vector2(
                    x: Screen.width,
                    y: Screen.width / imageRatio)
                : new Vector2(
                    x: Screen.height * imageRatio,
                    y: Screen.height);

            return result;
        }
    }
}