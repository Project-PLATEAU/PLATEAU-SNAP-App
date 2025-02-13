using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// テクスチャの情報を取得するリポジトリ
    /// テクスチャをシーンをまたいで共有するために使用
    /// </summary>
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

        /// <summary>
        /// テクスチャを設定
        /// </summary>
        public void SetTexture(Texture texture)
        {
            Clear();
            this.texture = texture;
        }

        /// <summary>
        /// テクスチャを取得
        /// </summary>
        public Texture GetTexture()
        {
            return texture;
        }

        /// <summary>
        /// テクスチャを破棄
        /// </summary>
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