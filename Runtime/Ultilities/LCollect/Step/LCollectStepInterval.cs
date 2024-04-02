using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepInterval : LCollectStep
    {
        [SerializeField] float _duration;

        public override string displayName { get { return "Interval"; } }

        public override void Apply(LCollectItem item)
        {
            item.sequence.AppendInterval(_duration);
        }
    }
}
