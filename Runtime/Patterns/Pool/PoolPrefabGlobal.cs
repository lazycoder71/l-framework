using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LFramework
{
    public static class PoolPrefabGlobal 
    {
        static Dictionary<GameObject, IObjectPool<GameObject>> _poolLookup = new Dictionary<GameObject, IObjectPool<GameObject>>();

        public static GameObject Get(GameObject prefab)
        {
            if (_poolLookup.ContainsKey(prefab))
                return _poolLookup[prefab].Get();

            _poolLookup.Add(prefab, new PoolPrefab(prefab));

            return null;
        }
    }
}
