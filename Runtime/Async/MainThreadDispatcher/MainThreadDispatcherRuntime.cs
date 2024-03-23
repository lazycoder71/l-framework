namespace LFramework
{
    class MainThreadDispatcherRuntime : MainThreadDispatcherBase
    {
        public override void Init()
        {
            MonoCallback.instance.eventUpdate += Update;
        }
    }
}
