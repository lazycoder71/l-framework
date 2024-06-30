using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework.View
{
    [Serializable]
    public class ViewExtra
    {
        public virtual string displayName { get; }

        [MinMaxSlider(0.0f, 1.0f, true)]
        [SerializeField] Vector2 _durationRange = new Vector2(0.0f, 1.0f);

        public void Apply(View view)
        {
            float animDuration = Mathf.Max(view.openDuration, view.closeDuration) * (_durationRange.y - _durationRange.x);
            float animDelay = Mathf.Max(view.openDuration, view.closeDuration) * _durationRange.x;

            Tween tween = GetTween(view, Mathf.Max(animDuration, 0.1f));

            if (animDelay > 0.0f)
                view.sequence.Insert(animDelay, tween);
            else
                view.sequence.Join(tween);
        }

        protected virtual Tween GetTween(View popup, float duration)
        {
            return null;
        }
    }
}
