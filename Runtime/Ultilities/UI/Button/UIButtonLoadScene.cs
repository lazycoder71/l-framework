using UnityEngine;

namespace LFramework
{
    public class UIButtonLoadScene : UIButtonBase
    {
        [Header("Config")]
        [SerializeField] int _sceneIndex;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            SceneLoaderHelper.Load(_sceneIndex);
        }
    }
}