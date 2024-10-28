using LFramework.ScriptableObjects;
using UnityEngine;

namespace LFramework.SceneLoader
{
    public class SceneLoaderSOS : ScriptableObjectSingleton<SceneLoaderSOS>
    {
        [SerializeField] private GameObject _prefab;

        public static GameObject Prefab => Instance._prefab;
    }
}
