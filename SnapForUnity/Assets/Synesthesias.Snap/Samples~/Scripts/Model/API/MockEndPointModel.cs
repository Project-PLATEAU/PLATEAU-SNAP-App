namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// エンドポイントのModel
    /// </summary>
    public class MockEndPointModel : IEndPointModel
    {
        /// <summary>
        /// エンドポイントを取得
        /// </summary>
        public string GetEndPoint()
        {
            var result = "http://localhost:8181/";
            return result;
        }
    }
}