using UnityEngine;

namespace LFramework.Vibration
{
    public static class Vibration
    {
        public static bool Enabled = true;

        ///<summary>
        /// Tiny pop vibration
        ///</summary>
        public static void VibratePop()
        {
            if (!Enabled)
                return;

#if UNITY_IOS
            VibrationIOS.VibratePop();
#elif UNITY_ANDROID
            VibrationAndroid.Vibrate(50);
#endif
        }

        ///<summary>
        /// Small peek vibration
        ///</summary>
        public static void VibratePeek()
        {
            if (!Enabled)
                return;

#if UNITY_IOS
            VibrationIOS.VibratePeek();
#elif UNITY_ANDROID
            VibrationAndroid.Vibrate(100);
#endif
        }

        ///<summary>
        /// 3 small vibrations
        ///</summary>
        public static void VibrateNope()
        {
            if (!Enabled)
                return;

#if UNITY_IOS
            VibrationIOS.VibrateNope();
#elif UNITY_ANDROID
            long[] pattern = { 0, 50, 50, 50 };
            VibrationAndroid.Vibrate(pattern);
#endif
        }

        public static void VibrateLight()
        {
            if (!Enabled)
                return;

#if UNITY_IOS
            VibrationIOS.Vibrate(VibrationIOS.ImpactFeedbackStyle.Light);
#elif UNITY_ANDROID
            VibrationAndroid.Vibrate(20, 40);
#endif
        }

        public static void VibrateMedium()
        {
            if (!Enabled)
                return;

#if UNITY_IOS
            VibrationIOS.Vibrate(VibrationIOS.ImpactFeedbackStyle.Medium);
#elif UNITY_ANDROID
            VibrationAndroid.Vibrate(40, 120);
#endif
        }

        public static void VibrateHeavy()
        {
            if (!Enabled)
                return;

#if UNITY_IOS
            VibrationIOS.Vibrate(VibrationIOS.ImpactFeedbackStyle.Heavy);
#elif UNITY_ANDROID
            VibrationAndroid.Vibrate(80, 255);
#endif
        }

        /// <summary>
        /// Default vibrations of device
        /// </summary>
        public static void Vibrate()
        {
            Handheld.Vibrate();
        }

        public static bool HasVibrator()
        {
#if UNITY_IOS
            return VibrationIOS.HasVibrator();
#elif UNITY_ANDROID
            return VibrationAndroid.HasVibrator();
#else
            return false;
#endif
        }
    }


}