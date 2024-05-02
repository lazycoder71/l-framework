using UnityEngine;

namespace LFramework
{
    public class PopupRootSetter : MonoBehaviour
    {
        private void Awake()
        {
            PopupManager.SetRoot(transform);
        }
    }
}
