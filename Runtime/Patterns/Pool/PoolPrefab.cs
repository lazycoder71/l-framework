using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LFramework
{
    public class PoolPrefab : ObjectPool<GameObject>
    {
        public PoolPrefab(GameObject prefab) : base(() => { return Object.Instantiate(prefab); }, (obj) => { obj.SetActive(true); }, (obj) => { obj.SetActive(false); })
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
            null,
            true,
            config.spawnCapacity, config.spawnCapacityMax
            )
        {
            List<GameObject> objSpawn = new List<GameObject>();

            for (int i = 0; i < config.spawnAtStart; i++)
                objSpawn.Add(Get());

            for (int i = 0; i < objSpawn.Count; i++)
                Release(objSpawn[i]);
        }
    }
}
