using System;
using UnityEngine;

namespace LFramework.LifetimeBinding
{
    /// <summary>
    ///     <see cref="ILifetimeBinding" /> that release when the GameObject is destroyed.
    /// </summary>
    public sealed class LifetimeBindingMonoBehaviour : MonoBehaviour, ILifetimeBinding
    {
        private void OnDestroy()
        {
            _eventRelease?.Invoke();
        }

        event Action ILifetimeBinding.EventRelease
        {
            add => _eventRelease += value;
            remove => _eventRelease -= value;
        }

        private event Action _eventRelease;
    }
}
