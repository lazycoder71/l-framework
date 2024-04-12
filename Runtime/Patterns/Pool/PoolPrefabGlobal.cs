using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LFramework
{
    public static class PoolPrefabGlobal
    {
        static Dictionary<GameObject, PoolPrefab> _poolLookup = new Dictionary<GameObject, PoolPrefab>();

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            MonoCallback.instance.eventActiveSceneChanged += MonoCallback_EventActiveSceneChanged;
        }

        private static void MonoCallback_EventActiveSceneChanged(Scene sceneCurrent, Scene sceneNext)
        {
            foreach (PoolPrefab pool in _poolLookup.Values)
            {
                if (pool.config != null && pool.config.dontDestroyOnLoad)
                    continue;

                pool.Clear();
            }
        }

        public static void Construct(params PoolPrefabConfig[] configs)
        {
            for (int i = 0; i < configs.Length; i++)
            {
                if (!_poolLookup.ContainsKey(configs[i].prefab))
                    _poolLookup.Add(configs[i].prefab, new PoolPrefab(configs[i]));
            }
        }

        public static GameObject Get(PoolPrefabConfig config)
        {
            return GetPool(config).Get();
        }

        public static GameObject Get(GameObject prefab)
        {
            return GetPool(prefab).Get();
        }

        public static void Release(PoolPrefabConfig config, GameObject instance)
        {
            GetPool(config).Release(instance);
        }

        public static void Release(GameObject prefab, GameObject instance)
        {
            GetPool(prefab).Release(instance);
        }

        public static PoolPrefab GetPool(PoolPrefabConfig config)
        {
            if (!_poolLookup.ContainsKey(config.prefab))
                _poolLookup.Add(config.prefab, new PoolPrefab(config));

            return _poolLookup[config.prefab];
        }

        public static PoolPrefab GetPool(GameObject prefab)
        {
            if (!_poolLookup.ContainsKey(prefab))
                _poolLookup.Add(prefab, new PoolPrefab(prefab));

            return _poolLookup[prefab];
        }
    }
}