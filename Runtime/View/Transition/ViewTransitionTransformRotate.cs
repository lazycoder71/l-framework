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
            Vector3 value = _keepDestination ? entity.rectTransform.anchoredPosition3D : _value;
            Vector3 valueStart = _keepStart ? entity.rectTransform.anchoredPosition3D : _valueStart;

            return entity.transformCached.DORotate(value, duration, _rotateMode)
                                         .ChangeStartValue(valueStart)
                                         .SetEase(_ease);
        }
    }
}
