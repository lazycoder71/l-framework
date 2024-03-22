using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Bounce.Framework
{
    /// <summary>
    /// Class that contains methods useful for debugging.
    /// All methods are only compiled if the DEVELOPMENT_BUILD symbol or UNITY_EDITOR is defined.
    /// </summary>
    public static class LDebug
    {
        #region Logs

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AssertIfTrue(bool condition, string message, params object[] args)
        {
            if (condition)
            {
                Debug.LogErrorFormat(message, args);
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void AssertIfFalse(bool condition, string message, params object[] args)
        {
            if (!condition)
            {
                Debug.LogErrorFormat(message, args);
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message, Color? color = null)
        {
            if (color.HasValue)
                Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGB(color.Value) + ">" + message.ToString() + "</color>");
            else
                Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string message, params object[] args)
        {
            Debug.LogFormat(message, args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string message, Color color, params object[] args)
        {
            Debug.LogFormat("<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + message + "</color>", args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log<T>(string message, params object[] args)
        {
            Debug.Log(string.Format("[{0}]:", typeof(T)) + string.Format(message, args));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(string message, params object[] args)
        {
            Debug.LogWarningFormat(message, args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(string message, params object[] args)
        {
            Debug.LogError(string.Format(message, args));
        }

        #endregion
    }
}