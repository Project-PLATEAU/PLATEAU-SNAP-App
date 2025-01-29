using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using VContainer.Unity;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// テキストのLocalizationのModel
    /// </summary>
    public class LocalizationModel : IAsyncStartable
    {
        private StringTable table;

        /// <summary>
        /// 開始
        /// </summary>
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // MEMO: 言語設定
            var locale = Locale.CreateLocale("ja");
            LocalizationSettings.SelectedLocale = locale;

            await LocalizationSettings
                .InitializationOperation
                .ToUniTask(cancellationToken: cancellation);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync(
            string tableName,
            CancellationToken cancellationToken)
        {
            if (table)
            {
                return;
            }

            table = await LocalizationSettings.StringDatabase
                .GetTableAsync(tableName)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// テキスト取得
        /// </summary>
        public string Get(string key)
        {
            var entry = table.GetEntry(key);
            var result = entry.GetLocalizedString();
            return result;
        }
    }
}