using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class LFactory : ScriptableObjectSingleton<LFactory>
    {
        [Title("Scene Loader")]
        [SerializeField] float _sceneLoaderFadeInDuration = 0.2f;
        [SerializeField] float _sceneLoaderLoadDuration = 0.1f;
        [SerializeField] float _sceneLoaderFadeOutDuration = 0.2f;
        [SerializeField] GameObject _sceneLoaderPrefab;

        public static float sceneTransitionFadeInDuration => instance._sceneLoaderFadeInDuration;
        public static float sceneTransitionLoadDuration => instance._sceneLoaderLoadDuration;
        public static float sceneTransitionFadeOutDuration => instance._sceneLoaderFadeOutDuration;
        public static GameObject sceneTransitionPrefab => instance._sceneLoaderPrefab;
    }
}