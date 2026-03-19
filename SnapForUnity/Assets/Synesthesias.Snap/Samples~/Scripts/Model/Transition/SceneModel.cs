using Cysharp.Threading.Tasks;
using System.Collections;
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

        /// <summary>
        /// シーンをAdditiveでロードし、アクティブシーンに設定する
        /// </summary>
        public async UniTask TransitionAdditive(string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // シーンの読み込みが完了するまで待機
            await UniTask.WaitUntil(() => asyncLoad.isDone);

            // 1フレーム待機してシーンの初期化を確実にする
            await UniTask.Yield();

            var loadedScene = SceneManager.GetSceneByName(sceneName);
            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
            }
        }

        /// <summary>
        /// 指定したシーンをアンロードする
        /// </summary>
        public void Unload(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        /// <summary>
        /// 指定シーンをアクティブに設定し、別のシーンをアンロードする
        /// </summary>
        public void UnloadAndSetActive(string activeSceneName, string unloadSceneName)
        {
            var activeScene = SceneManager.GetSceneByName(activeSceneName);
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }

            var unloadScene = SceneManager.GetSceneByName(unloadSceneName);
            if (unloadScene.IsValid())
            {
                SceneManager.UnloadSceneAsync(unloadScene);
            }
        }

        private static string GetActiveSceneName()
        {
            var result = SceneManager.GetActiveScene().name;
            return result;
        }
    }
}