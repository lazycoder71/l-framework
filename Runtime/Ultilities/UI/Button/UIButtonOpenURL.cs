using UnityEngine;

namespace LFramework
{
    public class UIButtonOpenURL : UIButtonBase
    {
        [SerializeField] private string _strURL;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            Application.OpenURL(_strURL);
        }
    }
}