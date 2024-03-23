using UnityEngine;

namespace LFramework
{
    public static class ColorExtensions
    {
        public static float Magnitude(this Color color)
        {
            return color.r + color.g + color.b + color.a;
        }
    }
}