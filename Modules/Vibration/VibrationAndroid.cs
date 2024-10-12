#if UNITY_ANDROID

using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace LFramework.Vibration
{
    /// <summary>
    /// Class for controlling Vibration on Android. Automatically initializes before scene is loaded.
    /// Original file: https://gist.github.com/ruzrobert/d98220a3b7f71ccc90403e041967c46b
    /// </summary>
    public static class VibrationAndroid
    {
        // Initialization flag
        private static bool _isInitialized = false;

        // Vibrator references
        private static AndroidJavaObject _vibrator = null;
        private static AndroidJavaClass _vibrationEffectClass = null;
        private static int _defaultAmplitude = 255;

        // Api level
        private static int _apiLevel = 1;

        private static bool IsSupportVibrationEffect()
        {
            // Available only from Api >= 26
            return _apiLevel >= 26;
        }

        private static bool IsSupportPredefinedEffect()
        {
            // Available only from Api >= 29
            return _apiLevel >= 29;
        }

        #region Initialization

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_isInitialized)
                return;

            // Add APP VIBRATION PERMISSION to the Manifest
            if (Application.isConsolePlatform)
                Handheld.Vibrate();

            // Get Api Level
            using (AndroidJavaClass androidVersionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                _apiLevel = androidVersionClass.GetStatic<int>("SDK_INT");
            }

            // Get UnityPlayer and CurrentActivity
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (currentActivity != null)
                {
                    _vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                    // If device supports vibration effects, get corresponding class
                    if (IsSupportVibrationEffect())
                    {
                        _vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                        _defaultAmplitude = Mathf.Clamp(_vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"), 1, 255);
                    }

                    // If device supports predefined effects, get their IDs
                    if (IsSupportPredefinedEffect())
                    {
                        PredefinedEffect.EFFECT_CLICK = _vibrationEffectClass.GetStatic<int>("EFFECT_CLICK");
                        PredefinedEffect.EFFECT_DOUBLE_CLICK = _vibrationEffectClass.GetStatic<int>("EFFECT_DOUBLE_CLICK");
                        PredefinedEffect.EFFECT_HEAVY_CLICK = _vibrationEffectClass.GetStatic<int>("EFFECT_HEAVY_CLICK");
                        PredefinedEffect.EFFECT_TICK = _vibrationEffectClass.GetStatic<int>("EFFECT_TICK");
                    }
                }
            }

            LDebug.Log(typeof(VibrationAndroid), $"Initialized" +
                $"\nDevice has Vibrator = {HasVibrator()}" +
                $"\nDevice support Amplitude Control = {HasAmplitudeControl()}" +
                $"\nDevice support Predefined Effects = {IsSupportPredefinedEffect()}");

            _isInitialized = true;
        }

        #endregion

        #region Vibrate Public

        /// <summary>
        /// Vibrate for Milliseconds, with Amplitude (if available).
        /// If amplitude is -1, amplitude is Disabled. If -1, device DefaultAmplitude is used. Otherwise, values between 1-255 are allowed.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void Vibrate(long milliseconds, int amplitude = -1, bool cancel = false)
        {
            // Lazy initialize
            Initialize();

            if (!HasVibrator())
                return;

            if (cancel)
                Cancel();

            if (IsSupportVibrationEffect())
            {
                // Validate amplitude
                amplitude = Mathf.Clamp(amplitude, -1, 255);

                // If -1, disable amplitude (use maximum amplitude)
                if (amplitude == -1)
                    amplitude = 255;

                // If 0, use device DefaultAmplitude
                if (amplitude == 0)
                    amplitude = _defaultAmplitude;

                // If amplitude is not supported, use 255; if amplitude is -1, use systems DefaultAmplitude. Otherwise use user-defined value.
                amplitude = !HasAmplitudeControl() ? 255 : amplitude;

                VibrateEffect(milliseconds, amplitude);
            }
            else
            {
                VibrateLegacy(milliseconds);
            }
        }

        /// <summary>
        /// Vibrate Pattern (pattern of durations, with format Off-On-Off-On and so on).
        /// Amplitudes can be Null (for default) or array of Pattern array length with values between 1-255.
        /// To cause the pattern to repeat, pass the index into the pattern array at which to start the repeat, or -1 to disable repeating.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void Vibrate(long[] pattern, int[] amplitudes = null, int repeat = -1, bool cancel = false)
        {
            // Lazy initialize
            Initialize();

            if (!HasVibrator())
                return;

            if (!HasAmplitudeControl())
                amplitudes = null;

            // Check Amplitudes array length
            if (amplitudes != null && amplitudes.Length != pattern.Length)
            {
                LDebug.LogWarning(typeof(VibrationAndroid), "Length of Amplitudes array is not equal to Pattern array. Amplitudes will be ignored.");

                amplitudes = null;
            }

            // Limit amplitudes between 1 and 255
            if (amplitudes != null)
                ClampAmplitudesArray(amplitudes);

            if (cancel)
                Cancel();

            if (IsSupportVibrationEffect())
            {
                if (amplitudes != null)
                    VibrateEffect(pattern, amplitudes, repeat);
                else
                    VibrateEffect(pattern, repeat);
            }
            else
            {
                VibrateLegacy(pattern, repeat);
            }
        }

        /// <summary>
        /// Vibrate predefined effect (described in Vibration.PredefinedEffect). Available from Api Level >= 29.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void VibratePredefined(int effectId, bool cancel = false)
        {
            // Lazy initialize
            Initialize();

            if (!HasVibrator())
                return;

            if (!IsSupportPredefinedEffect())
                return;

            if (cancel)
                Cancel();

            VibrateEffectPredefined(effectId);
        }

        #endregion

        #region Public Properties & Controls

        public static long[] ParsePattern(string pattern)
        {
            if (pattern == null)
                return new long[0];

            pattern = pattern.Trim();

            string[] split = pattern.Split(',');

            long[] timings = new long[split.Length];

            for (int i = 0; i < split.Length; i++)
            {
                if (int.TryParse(split[i].Trim(), out int duration))
                    timings[i] = duration < 0 ? 0 : duration;
                else
                    timings[i] = 0;
            }

            return timings;
        }

        /// <summary>
        /// Returns Android Api Level
        /// </summary>
        public static int GetApiLevel() => _apiLevel;

        /// <summary>
        /// Returns Default Amplitude of device, or 0.
        /// </summary>
        public static int GetDefaultAmplitude() => _defaultAmplitude;

        /// <summary>
        /// Returns true if device has vibrator
        /// </summary>
        public static bool HasVibrator()
        {
            return _vibrator != null && _vibrator.Call<bool>("hasVibrator");
        }

        /// <summary>
        /// Return true if device supports amplitude control
        /// </summary>
        public static bool HasAmplitudeControl()
        {
            if (HasVibrator() && IsSupportVibrationEffect())
                return _vibrator.Call<bool>("hasAmplitudeControl"); // API 26+ specific
            else
                return false; // no amplitude control below API level 26
        }

        /// <summary>
        /// Tries to cancel current vibration
        /// </summary>
        public static void Cancel()
        {
            if (HasVibrator())
                _vibrator.Call("cancel");
        }
        #endregion

        #region Vibrate Internal
        #region Vibration Callers
        private static void VibrateEffect(long milliseconds, int amplitude)
        {
            using (AndroidJavaObject effect = CreateEffect_OneShot(milliseconds, amplitude))
            {
                _vibrator.Call("vibrate", effect);
            }
        }

        private static void VibrateLegacy(long milliseconds)
        {
            _vibrator.Call("vibrate", milliseconds);
        }

        private static void VibrateEffect(long[] pattern, int repeat)
        {
            using (AndroidJavaObject effect = CreateEffect_Waveform(pattern, repeat))
            {
                _vibrator.Call("vibrate", effect);
            }
        }

        private static void VibrateLegacy(long[] pattern, int repeat)
        {
            _vibrator.Call("vibrate", pattern, repeat);
        }

        private static void VibrateEffect(long[] pattern, int[] amplitudes, int repeat)
        {
            using (AndroidJavaObject effect = CreateEffect_Waveform(pattern, amplitudes, repeat))
            {
                _vibrator.Call("vibrate", effect);
            }
        }

        private static void VibrateEffectPredefined(int effectId)
        {
            using (AndroidJavaObject effect = CreateEffect_Predefined(effectId))
            {
                _vibrator.Call("vibrate", effect);
            }
        }
        #endregion

        #region Vibration Effect
        /// <summary>
        /// Wrapper for public static VibrationEffect createOneShot (long milliseconds, int amplitude). API >= 26
        /// </summary>
        private static AndroidJavaObject CreateEffect_OneShot(long milliseconds, int amplitude)
        {
            return _vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
        }

        /// <summary>
        /// Wrapper for public static VibrationEffect createPredefined (int effectId). API >= 29
        /// </summary>
        private static AndroidJavaObject CreateEffect_Predefined(int effectId)
        {
            return _vibrationEffectClass.CallStatic<AndroidJavaObject>("createPredefined", effectId);
        }

        /// <summary>
        /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int[] amplitudes, int repeat). API >= 26
        /// </summary>
        private static AndroidJavaObject CreateEffect_Waveform(long[] timings, int[] amplitudes, int repeat)
        {
            return _vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, amplitudes, repeat);
        }

        /// <summary>
        /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int repeat). API >= 26
        /// </summary>
        private static AndroidJavaObject CreateEffect_Waveform(long[] timings, int repeat)
        {
            return _vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, repeat);
        }
        #endregion
        #endregion

        #region Internal
        private static void ClampAmplitudesArray(int[] amplitudes)
        {
            for (int i = 0; i < amplitudes.Length; i++)
            {
                amplitudes[i] = Mathf.Clamp(amplitudes[i], 1, 255);
            }
        }
        #endregion

        public static class PredefinedEffect
        {
            public static int EFFECT_CLICK;         // public static final int EFFECT_CLICK
            public static int EFFECT_DOUBLE_CLICK;  // public static final int EFFECT_DOUBLE_CLICK
            public static int EFFECT_HEAVY_CLICK;   // public static final int EFFECT_HEAVY_CLICK
            public static int EFFECT_TICK;          // public static final int EFFECT_TICK
        }
    }
}

#endif