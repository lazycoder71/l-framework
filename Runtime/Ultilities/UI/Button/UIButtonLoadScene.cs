using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class UIButtonLoadScene : UIButtonBase
    {
        [Title("Config")]
        [SerializeField] private int _sceneIndex;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            SceneLoaderHelper.Load(_sceneIndex);
        }
    }
}