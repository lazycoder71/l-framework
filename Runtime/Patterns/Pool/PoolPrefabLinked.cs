using UnityEngine;
using UnityEngine.Pool;

namespace LFramework
{
    public class PoolPrefabLinked : LinkedPool<GameObject>
    {
        public PoolPrefabLinked(GameObject prefab) : base(() => { return Object.Instantiate(prefab); }, (obj) => { obj.SetActive(true); }, (obj) => { obj.SetActive(false); })
        {

        }
    }
}
