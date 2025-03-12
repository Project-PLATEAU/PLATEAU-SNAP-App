using System;

namespace Synesthesias.PLATEAU.Snap.Generated
{
    /// <summary>
    /// 画像登録エラー
    /// </summary>
    public class BuildingImageException : Exception
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public readonly string Title;

        /// <summary>
        /// 説明
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BuildingImageException(
            string title,
            string description)
        {
            Title = title;
            Description = description;
        }
    }
}