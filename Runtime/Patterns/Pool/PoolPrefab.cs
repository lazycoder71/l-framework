using UnityEngine;
using UnityEngine.Pool;

namespace LFramework
{
    public class PoolPrefab : ObjectPool<GameObject>
    {
        public PoolPrefab(GameObject prefab) : base(() => { return Object.Instantiate(prefab); }, (obj) => { obj.SetActive(true); }, (obj) => { obj.SetActive(false); })
        {

        }
    }
}
