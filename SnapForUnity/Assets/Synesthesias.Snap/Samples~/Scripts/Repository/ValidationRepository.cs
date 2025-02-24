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
        private MeshValidationAngleResultType mockAngleResult;
        private MeshValidationVertexResultType mockVertexResult;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationRepository()
        {
            mockAngleResult = MeshValidationAngleResultType.None;
            mockVertexResult = MeshValidationVertexResultType.None;
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
            if (mockAngleResult != MeshValidationAngleResultType.None)
            {
                return mockAngleResult;
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
            if (mockVertexResult != MeshValidationVertexResultType.None)
            {
                return mockVertexResult;
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

        /// <summary>
        /// モックの角度判定の結果を設定
        /// </summary>
        public void SetMockAngleResult(MeshValidationAngleResultType mockAngleResult)
        {
            this.mockAngleResult = mockAngleResult;
        }

        /// <summary>
        /// モックの面判定の結果を設定
        /// </summary>
        public void SetMockVertexResult(MeshValidationVertexResultType mockVertexResult)
        {
            this.mockVertexResult = mockVertexResult;
        }
    }
}