using System;

namespace LFramework
{
    interface IMainThreadDispatcher
    {
        void Init();
        void Enqueue(Action action);
    }
}
