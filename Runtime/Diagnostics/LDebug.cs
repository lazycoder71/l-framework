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
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message, Color? color = null)
        {
            Debug.Log(GetLog(message, color.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object header, object message, Color? headerColor = null)
        {
            Debug.Log(GetLog(header, message, headerColor.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log<T>(object message, Color? headerColor = null)
        {
            Debug.Log(GetLog(typeof(T), message, headerColor.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message, Color? color = null)
        {
            Debug.LogWarning(GetLog(message, color.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object header, object message, Color? headerColor = null)
        {
            Debug.LogWarning(GetLog(header, message, headerColor.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning<T>(object message, Color? headerColor = null)
        {
            Debug.LogWarning(GetLog(typeof(T), message, headerColor.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object message, Color? color = null)
        {
            Debug.LogError(GetLog(message, color.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object header, object message, Color? headerColor = null)
        {
            Debug.LogError(GetLog(header, message, headerColor.GetValueOrDefault(Color.white)));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError<T>(object message, Color? headerColor = null)
        {
            Debug.LogError(GetLog(typeof(T), message, headerColor.GetValueOrDefault(Color.white)));
        }

        private static object GetLog(object message, Color color)
        {
            if (color == Color.white)
                return message;
            else
                return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>";
        }

        private static object GetLog(object header, object message, Color color)
        {
            if (color == Color.white)
                return $"[{header}] {message}";
            else
                return $"[<color=#{ColorUtility.ToHtmlStringRGB(color)}>{header}</color>] {message}";
        }
    }
}