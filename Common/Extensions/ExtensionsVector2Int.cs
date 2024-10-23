using UnityEngine;

namespace LFramework
{
    public static class ExtensionsVector2Int 
    {
        public static int RandomWithin(this Vector2Int v)
        {
            return Random.Range(v.x, v.y);
        }
    }
}