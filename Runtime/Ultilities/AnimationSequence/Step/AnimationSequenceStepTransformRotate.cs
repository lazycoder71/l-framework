using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepTransformRotate : AnimationSequenceStepTransform
    {
        [SerializeField]
        private RotateMode _rotateMode = RotateMode.Fast;

        public override string displayName { get { return $"{(_isSelf ? "Transform (This)" : _owner)}: DOLocalRotate"; } }

        protected override Tween GetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.transformCached : _owner;

            float duration = _isSpeedBased ? Vector3.Angle(_value, owner.localEulerAngles) / _duration : _duration;
            Vector3 start = _changeStartValue ? _valueStart : owner.localEulerAngles;
            Vector3 end = _relative ? owner.localEulerAngles + _value : _value;

            Tween tween = owner.DOLocalRotate(end, duration, _rotateMode)
                               .ChangeStartValue(start);

            owner.localEulerAngles = end;

            return tween;
        }

        protected override Tween GetResetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.transformCached : _owner;

            return owner.DORotate(owner.localEulerAngles, 0.0f);
        }
    }
}