using UnityEngine;

namespace LFramework
{
    public class UIPopupRootSetter : MonoBehaviour
    {
        void Awake()
        {
            UIPopupHelper.popupRoot = transform;
        }
    }
}