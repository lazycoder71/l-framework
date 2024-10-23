using LFramework.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LFramework
{
    public class SceneLoaderSOS : ScriptableObjectSingleton<SceneLoaderSOS>
    {
        [SerializeField] private GameObject _prefab;

        public static GameObject Prefab => Instance._prefab;
    }
}
