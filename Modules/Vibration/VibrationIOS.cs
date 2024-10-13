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

        public static void Vibrate(VibrationType type)
        {
            switch (type)
            {
                case VibrationType.Default:
                    Handheld.Vibrate();
                    break;

                case VibrationType.Tick:
                case VibrationType.ImpactLight:
                    Vibrate(ImpactFeedbackStyle.Light);
                    break;

                case VibrationType.ImpactMedium:
                    Vibrate(ImpactFeedbackStyle.Medium);
                    break;

                case VibrationType.ClickHeavy:
                case VibrationType.ImpactHeavy:
                    Vibrate(ImpactFeedbackStyle.Heavy);
                    break;

                case VibrationType.ClickDouble:
                case VibrationType.Rigid:
                    Vibrate(ImpactFeedbackStyle.Rigid);
                    break;

                case VibrationType.ClickSingle:
                case VibrationType.Soft:
                    Vibrate(ImpactFeedbackStyle.Soft);
                    break;

                case VibrationType.Success:
                    Vibrate(NotificationFeedbackStyle.Success);
                    break;

                case VibrationType.Failure:
                    Vibrate(NotificationFeedbackStyle.Error);
                    break;

                case VibrationType.Warning:
                    Vibrate(NotificationFeedbackStyle.Warning);
                    break;

                default:
                    LDebug.Log(typeof(VibrationIOS), $"Undefined vibrate type {type}");
                    break;
            }
        }
    }
}
#endif