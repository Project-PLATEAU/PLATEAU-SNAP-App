namespace Synesthesias.Snap.Sample
{
    public interface IApiKeyModel
    {
        /// <summary>
        /// キーの種類を取得
        /// 例: "Bearer", "x-api-key" 
        /// </summary>
        string GetKeyType();

        /// <summary>
        /// APIキーを取得
        /// </summary>
        string GetKeyValue();
    }
}