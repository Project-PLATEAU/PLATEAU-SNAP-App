namespace Synesthesias.PLATEAU.Snap.Generated.Model
{
    /// <summary>
    /// 画像登録APIのレスポンス
    /// </summary>
    public partial class BuildingImageResponse
    {
        /// <summary>
        /// エラーの場合は例外をスローする
        /// </summary>
        public void ThrowIfError()
        {
            if (Status == StatusType.Error)
            {
                throw new BuildingImageException(
                    title: "画像登録エラー",
                    description: Message);
            }
        }
    }
}