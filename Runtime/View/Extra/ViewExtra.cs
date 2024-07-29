using DG.Tweening;
using System;

namespace LFramework
{
    [Serializable]
    public abstract class ViewExtra
    {
        public virtual string displayName { get; }

        public void Apply(View view)
        {
            Tween tween = GetTween(view, 1.0f);

            view.sequence.Join(tween);
        }

        protected virtual Tween GetTween(View popup, float duration)
        {
            return null;
        }
    }
}
