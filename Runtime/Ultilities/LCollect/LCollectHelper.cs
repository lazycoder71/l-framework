using System;
using UnityEngine;

namespace LFramework
{
    public static class LCollectHelper
    {
        public static void Spawn(LCollectConfig config, int valueCount, Transform parent, Vector3 spawnPosition, Action onComplete)
        {
            GameObject objCollect = new GameObject(config.name);
            objCollect.AddComponent<RectTransform>();

            LCollect collect = objCollect.AddComponent<LCollect>();

            collect.transformCached.SetParent(parent, false);
            collect.transformCached.position = spawnPosition;

            collect.Construct(config, valueCount);

            collect.eventComplete += onComplete;
        }
    }
}