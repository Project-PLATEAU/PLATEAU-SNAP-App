using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.Snap.Runtime;
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
        [SerializeField] private EditorDetectionParameterView detectionParameterView;
        [SerializeField] private DetectionMenuView menuView;
        [SerializeField] private EditorDetectionMeshView detectionMeshViewTemplate;
        [SerializeField] private MeshView meshViewTemplate;
        [SerializeField] private RenderTexture renderTexture;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            ConfigureAPI(builder);
            ConfigureRepository(builder);

            builder.Register<EditorWebCameraModel>(Lifetime.Singleton)
                .WithParameter("renderTexture", renderTexture);

            ConfigureMenu(builder);
            ConfigureGeospatialMainLoop(builder);
            ConfigureGeospatial(builder);
            ConfigureAR(builder);
            ConfigureDetection(builder);
            ConfigureDetectionMesh(builder);
            ConfigureValidation(builder);
        }

        private void ConfigureAPI(IContainerBuilder builder)
        {
            var configuration = Parent.Container.Resolve<Synesthesias.PLATEAU.Snap.Generated.Client.Configuration>();
            var api = new SurfacesApi(configuration: configuration);

            builder.RegisterInstance(api)
                .AsImplementedInterfaces();
        }

        private void ConfigureRepository(IContainerBuilder builder)
        {
            builder.Register<SurfaceRepository>(Lifetime.Singleton);

            builder.Register<MeshRepository>(Lifetime.Singleton)
                .WithParameter("detectedMaterial", detectionView.DetectedMaterial)
                .WithParameter("selectedMaterial", detectionView.SelectedMaterial);
        }

        private void ConfigureMenu(IContainerBuilder builder)
        {
            builder.RegisterInstance(menuView);
            builder.Register<DetectionMenuModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DetectionMenuPresenter>();
        }

        private void ConfigureGeospatialMainLoop(IContainerBuilder builder)
        {
            //builder.RegisterInstance(geospatialMainLoopView);
            //builder.Register<GeospatialMainLoopModel>(Lifetime.Singleton);
            //builder.RegisterEntryPoint<GeospatialMainLoopPresenter>();
        }

        private static void ConfigureGeospatial(IContainerBuilder builder)
        {
            //builder.RegisterInstance(GeospatialAccuracyThresholdModel.Default);
            //builder.Register<GeospatialAccuracyModel>(Lifetime.Singleton);
            //builder.Register<GeospatialAnchorModel>(Lifetime.Singleton);
            //builder.Register<GeospatialPoseModel>(Lifetime.Singleton);
            // builder.Register<EditorGeospatialMathModel>(Lifetime.Singleton)
            //     .AsImplementedInterfaces();
            builder.Register<ShapeModel>(Lifetime.Singleton);
            builder.Register<EditorTriangulationModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder.Register<EditorGeospatialMeshModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private static void ConfigureAR(IContainerBuilder builder)
        {
            builder.Register<EditorGeospatialMathModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<MeshModel>(Lifetime.Singleton);
        }

        private void ConfigureDetection(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionView);

            builder.RegisterInstance(detectionParameterView)
                .AsImplementedInterfaces();

            builder.Register<EditorDetectionModel>(Lifetime.Singleton);
            //builder.RegisterEntryPoint<EditorDetectionPresenter>();
            builder.Register<DetectionTouchModel>(Lifetime.Singleton);
            builder.Register<MockValidationResultModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<EditorDetectionPresenter>();
        }

        private void ConfigureDetectionMesh(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionMeshViewTemplate);
            builder.RegisterInstance(meshViewTemplate);
            builder.Register<EditorDetectionMeshModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DetectionMeshPresenter>();
        }

        private void ConfigureValidation(IContainerBuilder builder)
        {
            builder.RegisterInstance(MeshValidationAngleThresholdModel.Default);

            builder.Register<MeshValidationModel>(Lifetime.Singleton)
                .WithParameter("camera", detectionView.MainCamera);
        }
    }
}