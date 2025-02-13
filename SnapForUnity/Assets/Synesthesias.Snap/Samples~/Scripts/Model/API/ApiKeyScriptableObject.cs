using System;
using UnityEngine;

namespace Synesthesias.Snap.Sample
{
    [CreateAssetMenu(menuName = "Synesthesias/Snap/Sample/ApiKeyScriptableObject")]
    [Obsolete(UnrecommendedMessage)]
    public class ApiConfigurationScriptableObject : ScriptableObject, IApiConfigurationModel
    {
        private const string UnrecommendedMessage = "APIキーのバージョン管理やアプリへの組込はセキュリティリスクがあります";

        [SerializeField] private string endPoint;
        [SerializeField] private string apiKeyType;
        [SerializeField] private string apiKeyValue;

        /// <summary>
        /// エンドポイントを取得
        /// </summary>
        public string EndPoint
            => endPoint;

        /// <summary>
        /// APIキーの種類を取得
        /// </summary>
        public string ApiKeyType
            => apiKeyType;

        /// <summary>
        /// APIキーを取得
        /// </summary>
        public string ApiKeyValue
            => apiKeyValue;
    }
}