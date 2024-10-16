using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepActionMove : LCollectStepAction
    {
        [Serializable]
        public enum Journey
        {
            Spawn = 0,
            Return = 1,
        }

        [SerializeField] protected Journey _journey;

        [Space]

        [ShowIf("@_journey == Journey.Spawn")]
        [SerializeField] protected bool _startAtCenter;
        [ShowIf("@_journey == Journey.Spawn && !_startAtCenter")]
        [SerializeField] protected Vector3 _startOffset;

        protected override Tween GetTween(LCollectItem item)
        {
            switch (_journey)
            {
                case Journey.Spawn:
                    Vector3 endPos = item.transform.localPosition;
                    Vector3 startPos = _startAtCenter ? Vector3.zero : endPos + _startOffset * item.rectTransform.GetUnitPerPixel();

                    return item.transform.DOLocalMove(endPos, _duration)
                                               .ChangeStartValue(startPos)
                                               .SetEase(_ease);
                case Journey.Return:
                    return item.transform.DOMove(item.destination.position, _duration)
                                               .SetEase(_ease);
                default:
                    return null;
            }
        }
    }
}
