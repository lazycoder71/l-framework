namespace LFramework.Pool
{
    public interface IPoolCallbackReceiver 
    {
        void OnGet();
        void OnRelease();
    }
}
