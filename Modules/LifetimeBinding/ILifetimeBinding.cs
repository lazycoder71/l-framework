using System;

namespace LFramework.LifetimeBinding
{
    public interface ILifetimeBinding
    {
        event Action EventRelease;
    }
}
