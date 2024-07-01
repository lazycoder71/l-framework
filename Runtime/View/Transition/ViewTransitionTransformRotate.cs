using DG.Tweening;
using UnityEngine;

namespace LFramework.View
{
    public class ViewTransitionTransformRotate : ViewTransitionTransform
    {
        [SerializeField] RotateMode _rotateMode = RotateMode.FastBeyond360;

        public override string displayName { get { return "Transform Rotate"; } }

        public override Tween GetTween(ViewTransitionEntity entity, float duration)
        {
            Vector3 value = _keepEnd ? entity.transformCached.localEulerAngles : _valueEnd;
            Vector3 valueStart = _keepStart ? entity.transformCached.localEulerAngles : _valueStart;

            return entity.transformCached.DOLocalRotate(value, duration, _rotateMode)
                                         .ChangeStartValue(valueStart)
                                         .SetEase(_ease);
        }
    }
}
