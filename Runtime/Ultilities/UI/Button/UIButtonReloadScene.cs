namespace LFramework
{
    public class UIButtonReloadScene : UIButtonBase
    {
        public override void Button_OnClick()
        {
            base.Button_OnClick();

            SceneLoaderHelper.Reload();
        }
    }
}