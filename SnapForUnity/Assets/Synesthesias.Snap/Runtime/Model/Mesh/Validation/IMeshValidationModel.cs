using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// メッシュの検証
    /// </summary>
    public interface IMeshValidationModel
    {
        /// <summary>
        /// メッシュを検証します
        /// </summary>
        MeshValidationResult Validate(
            Transform meshTransform,
            Mesh mesh);

        /// <summary>
        /// メッシュの角度の検証結果の種類
        /// </summary>
        MeshValidationAngleResultType GetMeshValidationAngleResultType(
            Transform meshTransform,
            Mesh mesh);

        /// <summary>
        /// メッシュの頂点の検証結果の種類を取得
        /// </summary>
        MeshValidationVertexResultType GetMeshValidationVertexResultType(
            Transform meshTransform,
            Mesh mesh);
    }
}