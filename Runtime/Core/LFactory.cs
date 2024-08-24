using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class LFactory : ScriptableObjectSingleton<LFactory>
    {
        [Title("Scene Loader")]
        [SerializeField] GameObject _sceneLoaderPrefab;

        public static GameObject sceneLoaderPrefab => instance._sceneLoaderPrefab;
    }
}