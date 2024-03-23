using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LFramework
{
    /// <summary>
    /// Class that contains methods useful for debugging.
    /// All methods are only compiled if the DEVELOPMENT_BUILD symbol or UNITY_EDITOR is defined.
    /// </summary>
    public static class LDebug
    {
        #region Logs

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message, Color color)
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object header, object message)
        {
            Debug.Log($"[{header}] {message}");
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object header, object message, Color headerColor)
        {
            Debug.Log($"[<color=#{ColorUtility.ToHtmlStringRGB(headerColor)}>{header}</color>] {message}");
        }

        public static void Log<T>(object message, Color? headerColor)
        {
            if (headerColor.HasValue)
                Log(typeof(T), message, headerColor.Value);
            else
                Log(typeof(T), message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        #endregion
    }
}