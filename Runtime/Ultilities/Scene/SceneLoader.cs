using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace LFramework
{
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        private static readonly float s_loadSceneProgressMax = 0.7f;

        [Title("Event")]
        [SerializeField] private UnityEvent _onFadeInStart;
        [SerializeField] private UnityEvent _onFadeInEnd;
        [SerializeField] private UnityEvent _onFadeOutStart;
        [SerializeField] private UnityEvent _onFadeOutEnd;

        [SerializeField] private UnityEvent<float> _onLoadProgress;

        [Title("Config")]
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _loadMinDuration;

        private bool _isTransiting = false;

        public float fadeInDuration { get { return _fadeInDuration; } }
        public float fadeOutDuration { get { return _fadeOutDuration; } }

        private async UniTaskVoid LoadAsync(AsyncOperation asyncOperation)
        {
            if (_isTransiting)
            {
                LDebug.Log<SceneLoader>("A scene is transiting, can't execute load scene command!");
                return;
            }

            gameObjectCached.SetActive(true);

            _isTransiting = true;

            // Progress event at 0
            _onLoadProgress?.Invoke(0f);

            // Fade in start
            _onFadeInStart?.Invoke();

            // Wait for fade in duration
            await UniTask.WaitForSeconds(_fadeInDuration, true);

            // Fade in end
            _onFadeInEnd?.Invoke();

            // Wait for scene load complete or min duration passed
            await WaitForSceneLoadedOrMinDuration(asyncOperation);

            // Fade out start
            _onFadeOutStart?.Invoke();

            // Wait for fade out duration
            await UniTask.WaitForSeconds(_fadeOutDuration, true);

            _onFadeOutEnd?.Invoke();

            _isTransiting = false;
        }

        private async UniTask WaitForSceneLoadedOrMinDuration(AsyncOperation handle)
        {
            var progress = Progress.CreateOnlyValueChanged<float>(x => _onLoadProgress?.Invoke(x * s_loadSceneProgressMax));

            float timeStartLoading = Time.unscaledTime;

            handle.allowSceneActivation = true;

            await handle.ToUniTask(progress);

            await Resources.UnloadUnusedAssets();

            float timeRemain = _loadMinDuration - (Time.unscaledTime - timeStartLoading);

            float time = 0f;

            while (time < timeRemain)
            {
                time += Time.unscaledDeltaTime;

                await UniTask.Yield();

                _onLoadProgress?.Invoke((time / timeRemain) * (1f - s_loadSceneProgressMax) + s_loadSceneProgressMax);
            }

            _onLoadProgress?.Invoke(1f);
        }

        #region Public

        public void Load(string sceneName)
        {
            LoadAsync(SceneManager.LoadSceneAsync(sceneName)).Forget();
        }

        public void Load(int sceneBuildIndex)
        {
            LoadAsync(SceneManager.LoadSceneAsync(sceneBuildIndex)).Forget();
        }

        #endregion
    }

    public static class SceneLoaderHelper
    {
        private static bool _isInitialized = false;

        private static void LazyInit()
        {
            if (_isInitialized)
                return;

            LFactory.sceneLoaderPrefab.Create();

            _isInitialized = true;
        }

        public static void Load(int sceneBuildIndex)
        {
            LazyInit();

            SceneLoader.instance.Load(sceneBuildIndex);
        }

        public static void Load(string sceneName)
        {
            LazyInit();

            SceneLoader.instance.Load(sceneName);
        }

        public static void Reload()
        {
            LazyInit();

            SceneLoader.instance.Load(SceneManager.GetActiveScene().buildIndex);
        }
    }
}