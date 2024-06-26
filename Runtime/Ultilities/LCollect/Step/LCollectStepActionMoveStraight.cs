using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepActionMoveStraight : LCollectStepActionMove
    {
        public override string displayName { get { return $"Move Straight ({_journey})"; } }

        protected override Tween GetTween(LCollectItem item)
        {
            switch (_journey)
            {
                case Journey.Spawn:
                    Vector3 endPos = item.transformCached.localPosition;
                    Vector3 startPos = _startAtCenter ? Vector3.zero : endPos + _startOffset * item.rectTransform.GetUnitPerPixel();

                    return item.transformCached.DOLocalMove(endPos, _duration)
                                               .ChangeStartValue(startPos)
                                               .SetEase(_ease);
                case Journey.Return:
                    return item.transformCached.DOMove(item.destination.position, _duration)
                                               .SetEase(_ease);
                default:
                    return null;
            }
        }
    }
}
