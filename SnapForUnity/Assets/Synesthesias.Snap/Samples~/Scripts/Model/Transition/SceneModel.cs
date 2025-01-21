using UnityEngine.SceneManagement;

namespace Synesthesias.Snap.Sample
{
    /// <summary>
    /// シーンのModel
    /// </summary>
    public class SceneModel
    {
        private string sceneName;

        /// <summary>
        /// 起動を通知する
        /// </summary>
        public void NotifyBoot()
        {
            var activeSceneName = GetActiveSceneName();

            // このシーンが初めて起動したシーンの場合は、起動シーンとして記憶する
            if (string.IsNullOrEmpty(sceneName))
            {
                sceneName = activeSceneName;
            }
        }

        /// <summary>
        /// 最初に起動したシーンかどうか
        /// </summary>
        public bool IsBootstrap()
        {
            var activeSceneName = GetActiveSceneName();
            var result = sceneName == activeSceneName;
            return result;
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        public void Transition(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private static string GetActiveSceneName()
        {
            var result = SceneManager.GetActiveScene().name;
            return result;
        }
    }
}