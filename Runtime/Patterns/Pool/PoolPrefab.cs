using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LFramework
{
    public class PoolPrefab : ObjectPool<GameObject>
    {
        PoolPrefabConfig _config;

        public PoolPrefabConfig config { get { return _config; } }

        public PoolPrefab(GameObject prefab) : base(
            () => { return Object.Instantiate(prefab); },
            (obj) => { obj.SetActive(true); },
            (obj) => { obj.SetActive(false); },
            (obj) => { LDebug.Log<PoolPrefab>($"Destroy {obj.name}"); },
#if UNITY_EDITOR
            true) // Keep heavy check on editor
#else
            false)
#endif
        {
        }

        public PoolPrefab(PoolPrefabConfig config) : base(
            () =>
            {
                GameObject createdObject = Object.Instantiate(config.prefab);

                if (config.dontDestroyOnLoad)
                    Object.DontDestroyOnLoad(createdObject);

                return createdObject;
            },
            (obj) => { obj.SetActive(true); },
            (obj) => { obj.SetActive(false); },
            (obj) => { LDebug.Log<PoolPrefab>($"Destroy {obj.name}"); },
#if UNITY_EDITOR
            true, // Keep heavy check on editor
#else
            false,
#endif
            config.spawnCapacity, config.spawnCapacityMax
            )
        {
            List<GameObject> objSpawn = new List<GameObject>();

            for (int i = 0; i < config.spawnAtStart; i++)
                objSpawn.Add(Get());

            for (int i = 0; i < objSpawn.Count; i++)
                Release(objSpawn[i]);

            _config = config;
        }
    }
}
