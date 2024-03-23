using TMPro;
using UnityEngine;

namespace LFramework
{
    public class UIPopupMessage : UIPopupBehaviour
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _txtContent;

        public void Construct(string msg)
        {
            _txtContent.text = msg;
        }
    }
}