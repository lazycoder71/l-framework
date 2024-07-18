using UnityEngine.UI;

namespace LFramework
{
    public class UIButtonBase : MonoCached
    {
        private Button _button;

        public Button button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                
                if(_button == null)
                    _button = GetComponentInChildren<Button>();

                return _button;
            }
        }

        protected virtual void Awake()
        {
            button.onClick.AddListener(Button_OnClick);
        }

        public virtual void Button_OnClick()
        {
        }
    }
}