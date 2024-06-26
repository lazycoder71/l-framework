using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepSpriteRenderer : AnimationSequenceStepAction<SpriteRenderer>
    {
        [SerializeField] private Color _value = Color.white;

        [SerializeField]
        [ShowIf("@_changeStartValue")]
        private Color _valueStart = Color.white;

        public override string displayName { get { return $"{(_isSelf ? "SpriteRenderer (This)" : _owner)}: DOColor"; } }

        protected override Tween GetTween(AnimationSequence animationSequence)
        {
            SpriteRenderer owner = _isSelf ? animationSequence.GetComponent<SpriteRenderer>() : _owner;

            float duration = _isSpeedBased ? Mathf.Abs(_value.Magnitude() - owner.color.Magnitude()) / _duration : _duration;
            Color start = _changeStartValue ? _valueStart : owner.color;
            Color end = _relative ? owner.color + _value : _value;

            Tween tween = owner.DOColor(end, duration)
                               .ChangeStartValue(start);

            owner.color = end;

            return tween;
        }

        protected override Tween GetResetTween(AnimationSequence animationSequence)
        {
            SpriteRenderer owner = _isSelf ? animationSequence.GetComponent<SpriteRenderer>() : _owner;

            return owner.DOColor(owner.color, 0.0f);
        }
    }
}