using Synesthesias.Snap.Runtime;

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
        /// 角度の検証結果のモック
        /// </summary>
        public MeshValidationAngleResultType MockAngleResult { get; set; }

        /// <summary>
        /// 頂点の検証結果のモック
        /// </summary>
        public MeshValidationVertexResultType MockVertexResult { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationRepository()
        {
            MockAngleResult = MeshValidationAngleResultType.None;
            MockVertexResult = MeshValidationVertexResultType.None;
        }

        /// <summary>
        /// パラメータを取得
        /// </summary>
        public ValidationParameterModel GetParameter()
        {
            return parameter;
        }

        /// <summary>
        /// 角度が正しいかどうかのモックを取得
        /// </summary>
        public MeshValidationAngleResultType GetAngleResult()
        {
            if (MockAngleResult != MeshValidationAngleResultType.None)
            {
                return MockAngleResult;
            }

            return parameter == null
                ? MeshValidationAngleResultType.None
                : parameter.MeshValidationResult.MeshAngleResultType;
        }

        /// <summary>
        /// 面が正しいかどうかのモックを取得
        /// </summary>
        public MeshValidationVertexResultType GetVertexResult()
        {
            if (MockVertexResult != MeshValidationVertexResultType.None)
            {
                return MockVertexResult;
            }

            return parameter == null
                ? MeshValidationVertexResultType.None
                : parameter.MeshValidationResult.MeshVertexResultType;
        }

        /// <summary>
        /// パラメータを設定
        /// </summary>
        public void SetParameter(ValidationParameterModel parameter)
        {
            this.parameter = parameter;
        }
    }
}