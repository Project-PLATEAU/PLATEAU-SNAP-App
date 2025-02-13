namespace Synesthesias.Snap.Sample
{
    public interface IApiConfigurationModel
    {
        /// <summary>
        /// APIのエンドポイント
        /// </summary>
        string EndPoint { get; }

        /// <summary>
        /// APIキーの種類
        /// </summary>
        string ApiKeyType { get; }

        /// <summary>
        /// APIキーの値
        /// </summary>
        string ApiKeyValue { get; }
    }
}