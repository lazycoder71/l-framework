using UnityEngine;

namespace LFramework
{
    [System.Serializable]
    public class PoolPrefabConfig : ScriptableObject
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] bool _dontDestroyOnLoad;

        [Space]

        [SerializeField] int _spawnAtStart = 0;
        [SerializeField] int _spawnCapacity = 10;
        [SerializeField] int _spawnCapacityMax = 1000;

        public GameObject prefab { get { return _prefab; } }
        public bool dontDestroyOnLoad { get { return dontDestroyOnLoad; } }

        public int spawnAtStart { get { return _spawnAtStart; } }
        public int spawnCapacity { get { return _spawnCapacity; } }
        public int spawnCapacityMax { get { return _spawnCapacityMax; } }
    }
}