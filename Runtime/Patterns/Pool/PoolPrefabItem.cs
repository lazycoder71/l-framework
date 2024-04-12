using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LFramework
{
    public class PoolPrefabItem : MonoCached
    {
        [Title("Config")]
        [SerializeField] PoolPrefabConfig _config;

        private void Start()
        {
            if (_config.dontDestroyOnLoad)
                MonoCallback.instance.eventActiveSceneChanged += MonoCallback_EventActiveSceneChanged;
        }

        private void MonoCallback_EventActiveSceneChanged(Scene arg1, Scene arg2)
        {
            if (gameObjectCached.activeInHierarchy)
                PoolPrefabGlobal.Release(_config, gameObjectCached);
        }
    }
}
