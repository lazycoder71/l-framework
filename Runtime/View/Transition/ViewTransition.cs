using DG.Tweening;
using System;
using UnityEngine;

namespace LFramework.View
{
    [Serializable]
    public abstract class ViewTransition
    {
        [SerializeField] protected Ease _ease;

        public virtual string displayName { get; }

        public virtual Tween GetTween(ViewTransitionEntity entity, float duration)
        {
            return null;
        }
    }
}
