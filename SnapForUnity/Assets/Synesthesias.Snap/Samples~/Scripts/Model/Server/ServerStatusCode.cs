namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// サーバーからのレスポンスのステータスコード
    /// </summary>
    public enum ServerStatusCode
    {
        Success = 200, // 成功
        BadRequest = 400, // リクエストが不正
        NotFound = 404, // リソースが見つからない
        MethodNotAllowed = 405, // 許可されていないメソッド
        InternalServerError = 500, // サーバー内部エラー
        ServiceUnavailable = 503, // サービス利用不可
    }
}