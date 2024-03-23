using UnityEngine;

namespace LFramework
{
    public class UIButtonOpenURL : UIButtonBase
    {
        [SerializeField] string _strURL;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            Application.OpenURL(_strURL);
        }
    }
}