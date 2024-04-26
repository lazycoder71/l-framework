using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    [Serializable]
    public class PopupAnimation
    {
        public virtual string displayName { get; }

        [MinMaxSlider(0.0f, 1.0f, true)]
        [SerializeField] Vector2 _durationRange = new Vector2(0.0f, 1.0f);

        public void Apply(Popup popup, Sequence sequence)
        {
            float animDuration = popup.openDuration * (_durationRange.y - _durationRange.x);
            float animDelay = popup.openDuration * _durationRange.x;

            Tween tween = GetTween(popup, animDuration);

            if (animDelay > 0.0f)
                sequence.Insert(animDelay, tween);
            else
                sequence.Join(tween);
        }

        protected virtual Tween GetTween(Popup popup, float duration)
        {
            return null;
        }
    }
}
