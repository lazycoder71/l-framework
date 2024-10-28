using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepActionScale : LCollectStepAction
    {
        [SerializeField] Vector3 _value = Vector3.one;

        public override string DisplayName { get { return "Scale"; } }

        protected override Tween GetTween(LCollectItem item)
        {
            return item.transform.DOScale(_value, _duration)
                                       .SetEase(_ease);
        }
    }
}
