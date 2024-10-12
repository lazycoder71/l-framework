////////////////////////////////////////////////////////////////////////////////
//
// @author Benoît Freslon @benoitfreslon
// https://github.com/BenoitFreslon/Vibration
// https://benoitfreslon.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace LFramework.Vibration
{
    public static class Vibration
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern bool _HasVibrator();

        [DllImport("__Internal")]
        private static extern void _Vibrate();

        [DllImport("__Internal")]
        private static extern void _VibratePop();

        [DllImport("__Internal")]
        private static extern void _VibratePeek();

        [DllImport("__Internal")]
        private static extern void _VibrateNope();

        [DllImport("__Internal")]
        private static extern void _impactOccurred(string style);

        [DllImport("__Internal")]
        private static extern void _notificationOccurred(string style);

        [DllImport("__Internal")]
        private static extern void _selectionChanged();
#endif

#if UNITY_ANDROID
        private static AndroidJavaClass s_unityPlayer;
        private static AndroidJavaObject s_currentActivity;
        private static AndroidJavaObject s_vibrator;
        private static AndroidJavaObject s_context;

        private static AndroidJavaClass s_vibrationEffect;
#endif

        private static bool s_initialized = false;

        public static bool Enabled = false;

        public static void Init()
        {
            if (s_initialized)
                return;

#if UNITY_ANDROID

            if (Application.isMobilePlatform)
            {
                s_unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                s_currentActivity = s_unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                s_vibrator = s_currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                s_context = s_currentActivity.Call<AndroidJavaObject>("getApplicationContext");

                if (AndroidVersion >= 26)
                {
                    s_vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
                }
            }
#endif

            s_initialized = true;
        }

#if UNITY_IOS
        public static void VibrateIOS(ImpactFeedbackStyle style)
        {
            if (!Application.isMobilePlatform || !Enabled) return;
            _impactOccurred(style.ToString());
        }

        public static void VibrateIOS(NotificationFeedbackStyle style)
        {
            if (!Application.isMobilePlatform || !Enabled) return;
            _notificationOccurred(style.ToString());
        }

        public static void VibrateIOS_SelectionChanged()
        {
            if (!Application.isMobilePlatform || !Enabled) return;
            _selectionChanged();
        }
#endif


        ///<summary>
        /// Tiny pop vibration
        ///</summary>
        public static void VibratePop()
        {
#if UNITY_IOS
            if (!Application.isMobilePlatform || !Enabled) return;
            _VibratePop();
#elif ANDROID
            VibrateAndroid(50);
#endif
        }

        ///<summary>
        /// Small peek vibration
        ///</summary>
        public static void VibratePeek()
        {
#if UNITY_IOS
            if (!Application.isMobilePlatform || !Enabled) return;
            _VibratePeek();
#elif ANDROID
            VibrateAndroid(100);
#endif
        }

        ///<summary>
        /// 3 small vibrations
        ///</summary>
        public static void VibrateNope()
        {
#if UNITY_IOS
            if (!Application.isMobilePlatform || !Enabled) return;
            _VibrateNope();
#elif ANDROID
            long[] pattern = { 0, 50, 50, 50 };
            VibrateAndroid(pattern, -1);
#endif
        }


#if UNITY_ANDROID
        ///<summary>
        /// Only on Android
        /// https://developer.android.com/reference/android/os/Vibrator.html#vibrate(long)
        ///</summary>
        public static void VibrateAndroid(long milliseconds)
        {
            if (!Application.isMobilePlatform || !Enabled) return;

            if (AndroidVersion >= 26)
            {
                AndroidJavaObject createOneShot = s_vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, -1);
                s_vibrator.Call("vibrate", createOneShot);
            }
            else
            {
                s_vibrator.Call("vibrate", milliseconds);
            }
        }

        ///<summary>
        /// Only on Android
        /// https://proandroiddev.com/using-vibrate-in-android-b0e3ef5d5e07
        ///</summary>
        public static void VibrateAndroid(long[] pattern, int repeat)
        {
            if (!Application.isMobilePlatform || !Enabled) return;

            if (AndroidVersion >= 26)
            {
                //long[] amplitudes;
                AndroidJavaObject createWaveform = s_vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                s_vibrator.Call("vibrate", createWaveform);
            }
            else
            {
                s_vibrator.Call("vibrate", pattern, repeat);
            }
        }
#endif

        ///<summary>
        ///Only on Android
        ///</summary>
        public static void CancelAndroid()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                s_vibrator.Call("cancel");
#endif
            }
        }

        public static bool HasVibrator()
        {
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID

                AndroidJavaClass contextClass = new AndroidJavaClass("android.content.Context");
                string contextVibratorService = contextClass.GetStatic<string>("VIBRATOR_SERVICE");
                AndroidJavaObject systemService = s_context.Call<AndroidJavaObject>("getSystemService", contextVibratorService);
                if (systemService.Call<bool>("hasVibrator"))
                {
                    return true;
                }

                return false;

#elif UNITY_IOS
                return _HasVibrator();
#else
                return false;
#endif
            }
            else
            {
                return false;
            }
        }

        public static void Vibrate()
        {
#if UNITY_ANDROID || UNITY_IOS

            if (!Application.isMobilePlatform || !Enabled)
                return;

            Handheld.Vibrate();

#endif
        }

        private static int AndroidVersion
        {
            get
            {
                int iVersionNumber = 0;
                if (Application.platform == RuntimePlatform.Android)
                {
                    string androidVersion = SystemInfo.operatingSystem;
                    int sdkPos = androidVersion.IndexOf("API-");
                    iVersionNumber = int.Parse(androidVersion.Substring(sdkPos + 4, 2).ToString());
                }

                return iVersionNumber;
            }
        }
    }

    public enum ImpactFeedbackStyle
    {
        Heavy,
        Medium,
        Light,
        Rigid,
        Soft
    }

    public enum NotificationFeedbackStyle
    {
        Error,
        Success,
        Warning
    }
}