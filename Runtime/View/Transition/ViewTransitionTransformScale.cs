using DG.Tweening;
using UnityEngine;

namespace LFramework.View
{
    public class ViewTransitionTransformScale : ViewTransitionTransform
    {
        public override string displayName { get { return "RectTransform Scale"; } }

        public override Tween GetTween(ViewTransitionEntity entity, float duration)
        {
            Vector3 value = _keepEnd ? entity.transformCached.localScale : _valueEnd;
            Vector3 valueStart = _keepStart ? entity.transformCached.localScale : _valueStart;

            return entity.transformCached.DOScale(value, duration)
                                         .ChangeStartValue(valueStart)
                                         .SetEase(_ease);
        }
    }
}