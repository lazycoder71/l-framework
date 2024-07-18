using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class ViewTransitionTransformMove : ViewTransitionTransform
    {
        public override string displayName { get { return "Transform Move"; } }

        public override Tween GetTween(ViewTransitionEntity entity, float duration)
        {
            Vector3 value = _keepEnd ? entity.rectTransform.anchoredPosition3D : _valueEnd;
            Vector3 valueStart = _keepStart ? entity.rectTransform.anchoredPosition3D : _valueStart;

            return entity.rectTransform.DOAnchorPos3D(value, duration)
                                       .ChangeStartValue(valueStart)
                                       .SetEase(_ease);
        }
    }
}
