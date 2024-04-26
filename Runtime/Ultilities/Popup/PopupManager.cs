using System;
using System.Collections.Generic;
using UnityEngine;

namespace LFramework
{
    public static class PopupManager
    {
        // Global stack of popup
        static List<Popup> _mainStack = new List<Popup>();

        static Transform _root;

        public static Transform root
        {
            get
            {
                if (_root == null)
                    _root = UnityEngine.Object.FindObjectOfType<Canvas>().transform;

                return _root;
            }
        }

        public static event Action<Popup> eventPopupOpened;
        public static event Action<Popup> eventPopupClosed;

        public static void PushToStack(Popup popup)
        {
            // Disable current top popup
            if (_mainStack.Count > 0)
                _mainStack.Last().SetEnabled(false);

            // Add this popup into stack
            _mainStack.Add(popup);

            eventPopupOpened?.Invoke(popup);
        }

        public static void PopFromStack(Popup popup)
        {
            if (_mainStack.Count == 0)
            {
                LDebug.LogWarning(typeof(PopupManager), "There is no popup in stack");

                return;
            }
            else if (_mainStack.Last() != popup)
            {
                LDebug.LogWarning(typeof(PopupManager), $"This popup {popup} is not on top of the stack! try to remove it from stack anyway");

                _mainStack.Remove(popup);

                return;
            }

            // Pop top popup from stack
            _mainStack.RemoveAt(_mainStack.Count - 1);

            // Get current top popup and enable it
            if (_mainStack.Count > 0)
                _mainStack.Last().SetEnabled(true);

            eventPopupClosed?.Invoke(popup);
        }
    }
}
