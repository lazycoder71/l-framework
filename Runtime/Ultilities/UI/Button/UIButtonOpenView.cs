using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LFramework
{
    public class UIButtonOpenView : UIButtonBase
    {
        [Title("Config")]
        [SerializeField] protected AssetReferenceGameObject _view;

        public event Action<View> eventViewOpened;

        public override async void Button_OnClick()
        {
            base.Button_OnClick();

            View view = await ViewHelper.PushAsync(_view);

            eventViewOpened?.Invoke(view);

            OnViewOpened(view);
        }

        protected virtual void OnViewOpened(View view)
        {
        }
    }
}
