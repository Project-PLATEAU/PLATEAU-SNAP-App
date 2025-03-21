using Synesthesias.PLATEAU.Snap.Generated.Api;
using Synesthesias.Snap.Runtime;
using System;
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
        [SerializeField] private RenderTexture renderTexture;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            var environment = Parent.Container.Resolve<IEnvironmentModel>();

            ConfigureAPI(builder);
            ConfigureRepository(builder);

            builder.Register<EditorWebCameraModel>(Lifetime.Singleton)
                .WithParameter("renderTexture", renderTexture);

            ConfigureMenu(builder);
            ConfigureGeospatial(builder);
            ConfigureDetection(builder);
            ConfigureDetectionMesh(builder, environment);
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

            builder.Register<DetectionMaterialModel>(Lifetime.Singleton)
                .WithParameter("detectedMaterial", detectionView.DetectedMaterial)
                .WithParameter("selectableMaterial", detectionView.SelectableMaterial)
                .WithParameter("selectedMaterial", detectionView.SelectedMaterial);

            builder.Register<MeshRepository>(Lifetime.Singleton);
        }

        private void ConfigureMenu(IContainerBuilder builder)
        {
            builder.RegisterInstance(menuView);
            builder.Register<DetectionMenuModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DetectionMenuPresenter>();
        }

        private static void ConfigureGeospatial(IContainerBuilder builder)
        {
            builder.RegisterInstance(GeospatialAccuracyThresholdModel.Default);
            builder.Register<GeospatialAccuracyModel>(Lifetime.Singleton);
            builder.Register<GeospatialAnchorModel>(Lifetime.Singleton);
            builder.Register<GeospatialPoseModel>(Lifetime.Singleton);

            builder.Register<EditorGeospatialMathModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<EditorGeospatialMeshModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private void ConfigureDetection(IContainerBuilder builder)
        {
            builder.RegisterInstance(detectionView);

            builder.RegisterInstance(detectionParameterView)
                .AsImplementedInterfaces();

            builder.Register<EditorDetectionModel>(Lifetime.Singleton);
            builder.Register<DetectionTouchModel>(Lifetime.Singleton);
            builder.Register<MockValidationResultModel>(Lifetime.Singleton);
            builder.RegisterEntryPoint<EditorDetectionPresenter>();
        }

        private void ConfigureDetectionMesh(
            IContainerBuilder builder,
            IEnvironmentModel environmentModel)
        {
            builder.Register<VectorCalculatorModel>(Lifetime.Singleton);
            builder.Register<ShapeValidatorModel>(Lifetime.Singleton);

            switch (environmentModel.DetectionMeshType)
            {
                case DetectionMeshType.None:
                    throw new InvalidOperationException(
                        $"DetectionMeshTypeが未設定です: {environmentModel.DetectionMeshType}");
                case DetectionMeshType.Simple:
                    builder.Register<SimpleMeshFactoryModel>(Lifetime.Singleton)
                        .AsImplementedInterfaces();
                    break;
                case DetectionMeshType.iShape:
                    builder.Register<PlainShapeFactoryModel>(Lifetime.Singleton);

                    builder.Register<ShapeMeshFactoryModel>(Lifetime.Singleton)
                        .AsImplementedInterfaces();
                    break;
                default:
                    throw new NotImplementedException(
                        $"未実装のDetectionMeshTypeです: {environmentModel.DetectionMeshType}");
            }

            builder.RegisterInstance(detectionMeshViewTemplate);
            builder.Register<EditorDetectionMeshModel>(Lifetime.Singleton);
        }

        private void ConfigureValidation(IContainerBuilder builder)
        {
            builder.RegisterInstance(MeshValidationAngleThresholdModel.Default);

            builder.Register<EditorMeshValidationModel>(Lifetime.Singleton)
                .WithParameter("camera", detectionView.MainCamera)
                .AsImplementedInterfaces();
        }
    }
}