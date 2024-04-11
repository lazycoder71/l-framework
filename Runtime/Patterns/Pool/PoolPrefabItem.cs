using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class PoolPrefabItem : MonoBehaviour
    {
        [Title("Config")]
        [SerializeField, AssetsOnly] PoolPrefabConfig _config;

        [SerializeField] bool _releasePoolOnDisable = true;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }
    }
}
