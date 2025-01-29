using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// 建物検出画面のみが影響範囲のLifetimeScope
    /// </summary>
    public class DetectionLifetimeScope : BaseLifetimeScope
    {
        [SerializeField] private DetectionView detectionView;
        [SerializeField] private ValidationDialogView dialogPrefab;
        [SerializeField] private Transform dialogTransform;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            // TODO: DetectionModelを登録
            builder.Register<MockDetectionModel>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.RegisterInstance(detectionView);
            builder.RegisterEntryPoint<DetectionPresenter>();
            ConfigureDialog(builder);
        }

        private void ConfigureDialog(IContainerBuilder builder)
        {
            builder.Register<ValidationDialogModel>(Lifetime.Singleton);

            builder.RegisterComponentInNewPrefab(dialogPrefab, Lifetime.Singleton)
                .UnderTransform(dialogTransform);

            builder.RegisterEntryPoint<ValidationDialogPresenter>();
        }
    }
}