using UnityEngine;

namespace LFramework
{
    public static class LCollectHelper
    {
        public static void Spawn(LCollectConfig config, int valueCount, Transform target, Vector3 position)
        {
            GameObject objCollect = new GameObject(config.name);
            objCollect.AddComponent<RectTransform>();

            LCollect collect = objCollect.AddComponent<LCollect>();

            collect.transformCached.SetParent(target, false);
            collect.transformCached.position = position;

            collect.Construct(config, valueCount);
        }
    }
}