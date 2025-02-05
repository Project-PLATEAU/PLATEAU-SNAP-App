using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のみが影響範囲のLifetimeScope(エディタ用)
    /// </summary>
    public class EditorDetectionLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private EditorDetectionView detectionView;
        [SerializeField] private DetectionMenuView menuView;
        [SerializeField] private EditorDetectionMeshView detectionMeshViewTemplate;
        [SerializeField] private RenderTexture renderTexture;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<EditorWebCameraModel>(Lifetime.Singleton)
                .WithParameter("renderTexture", renderTexture);

            ConfigureMenu(builder);
            ConfigureDetection(builder);
            ConfigureDetectionMesh(builder);
        }

        private void ConfigureMenu(IContainerBuilder builder)
        {
            builder.RegisterInstance(menuView);
            builder.Register<DetectionMenuModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DetectionMenuPresenter>();
        }

        private void ConfigureDetection(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionView);
            builder.Register<EditorDetectionModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<EditorDetectionPresenter>();

            builder.Register<DetectionTouchModel>(Lifetime.Singleton)
                .WithParameter("detectedMaterial", detectionView.DetectedMaterial)
                .WithParameter("selectedMaterial", detectionView.SelectedMaterial);
        }

        private void ConfigureDetectionMesh(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionMeshViewTemplate);
            builder.Register<EditorMeshModel>(Lifetime.Singleton);
        }
    }
}