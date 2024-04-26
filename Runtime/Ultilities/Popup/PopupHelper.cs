using UnityEngine;

namespace LFramework
{
    public static class PopupHelper
    {
        public static Popup Open(GameObject prefab)
        {
            Popup popup = prefab.Create(PopupManager.root, false).GetComponent<Popup>();
            popup.transformCached.SetAsLastSibling();

            return popup;
        }
    }
}