using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    public class TextureRepository : IDisposable
    {
        private Texture texture;

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Clear();
        }

        public void SetTexture(Texture texture)
        {
            Clear();
            this.texture = texture;
        }

        public Texture GetTexture()
        {
            return texture;
        }

        public void Clear()
        {
            if (texture == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(texture);
            texture = null;
        }
    }
}