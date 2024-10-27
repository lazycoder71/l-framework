using DG.Tweening;

namespace LFramework.View
{
    [System.Serializable]
    public abstract class ViewExtra 
    {
        public abstract string DisplayName { get; }

        public void Apply(View view)
        {
            Tween tween = GetTween(view, 1.0f);

            view.Sequence.Join(tween);
        }

        protected abstract Tween GetTween(View popup, float duration);
    }
}
