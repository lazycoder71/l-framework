using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepTransformScale : AnimationSequenceStepTransform
    {
        public override string displayName { get { return $"{(_isSelf ? "Transform (This)" : _owner)}: DOScale"; } }

        protected override Tween GetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.transformCached : _owner;

            float duration = _isSpeedBased ? Vector3.Distance(_value, owner.localScale) / _duration : _duration;
            Vector3 start = _changeStartValue ? _valueStart : owner.localScale;
            Vector3 end = _relative ? owner.localScale + _value : _value;

            Tween tween = owner.DOScale(end, duration)
                               .ChangeStartValue(start);

            owner.localScale = end;

            return tween;
        }

        protected override Tween GetResetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.transformCached : _owner;

            return owner.DOScale(owner.localScale, 0.0f);
        }
    }
}