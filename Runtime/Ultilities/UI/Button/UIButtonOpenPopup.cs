using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class UIButtonOpenPopup : UIButtonBase
    {
        [Title("Config")]
        [SerializeField] private GameObject _popup;

        public event Action<Popup> eventSpawnPopup;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            SpawnPopup();
        }

        protected virtual void HandleSpawnPopup(Popup popupBehaviour)
        {

        }

        public Popup SpawnPopup()
        {
            Popup popup = PopupManager.Create(_popup);

            eventSpawnPopup?.Invoke(popup);

            HandleSpawnPopup(popup);

            return popup;
        }
    }
}