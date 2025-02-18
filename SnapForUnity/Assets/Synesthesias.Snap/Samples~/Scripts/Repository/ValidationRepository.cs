namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 検証シーンに情報を受け渡すためのリポジトリ
    /// パラメータをシーンをまたいで共有するために使用
    /// </summary>
    public class ValidationRepository
    {
        private ValidationParameterModel parameter;

        /// <summary>
        /// パラメータを設定
        /// </summary>
        public void SetParameter(ValidationParameterModel parameter)
        {
            this.parameter = parameter;
        }

        /// <summary>
        /// パラメータを取得
        /// </summary>
        public ValidationParameterModel GetParameter()
        {
            return parameter;
        }
    }
}