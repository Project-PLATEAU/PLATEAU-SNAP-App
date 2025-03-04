using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のマテリアルのModel
    /// </summary>
    public class DetectionMaterialModel
    {
        /// <summary>
        /// 検出時のメッシュのマテリアル
        /// </summary>
        public readonly Material DetectedMaterial;

        /// <summary>
        /// 選択可能なメッシュのマテリアル
        /// </summary>
        public readonly Material SelectableMaterial;

        /// <summary>
        /// 選択時のメッシュのマテリアル
        /// </summary>
        public readonly Material SelectedMaterial;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectionMaterialModel(
            Material detectedMaterial,
            Material selectableMaterial,
            Material selectedMaterial)
        {
            DetectedMaterial = detectedMaterial;
            SelectableMaterial = selectableMaterial;
            SelectedMaterial = selectedMaterial;
        }
    }
}