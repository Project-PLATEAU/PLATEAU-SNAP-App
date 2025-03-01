using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// 頂点座標を格納する構造体
    /// </summary>
    public struct Shape
    {
        /// <summary>
        /// 面の頂点座標の配列
        /// </summary>
        public Vector2[] Hull { get; }

        /// <summary>
        /// 穴の頂点座標の配列
        /// </summary>
        public Vector2[][] Holes { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Shape(
            Vector2[] hull,
            Vector2[][] holes = null)
        {
            Hull = hull;
            Holes = holes;
        }
    }
}