#if UNITY_IOS
using UnityEngine;
using System.Runtime.InteropServices;

namespace LFramework.Vibration
{
    public static class VibrationIOS
    {
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

        public static void Vibrate(ImpactFeedbackStyle style)
        {
            _impactOccurred(style.ToString());
        }

        public static void Vibrate(NotificationFeedbackStyle style)
        {
            _notificationOccurred(style.ToString());
        }

        public static void Vibrate_SelectionChanged()
        {
            _selectionChanged();
        }

        ///<summary>
        /// Tiny pop vibration
        ///</summary>
        public static void VibratePop()
        {
            _VibratePop();
        }

        ///<summary>
        /// Small peek vibration
        ///</summary>
        public static void VibratePeek()
        {
            _VibratePeek();
        }

        ///<summary>
        /// 3 small vibrations
        ///</summary>
        public static void VibrateNope()
        {
            _VibrateNope();
        }

        public static bool HasVibrator()
        {
            return _HasVibrator();
        }

        public static void Vibrate()
        {
            Handheld.Vibrate();
        }
    }
}
#endif