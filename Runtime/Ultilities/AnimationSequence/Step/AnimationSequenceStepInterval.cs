using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepInterval : AnimationSequenceStep
    {
        [SerializeField] 
        private float _duration;

        public override string displayName { get { return "Interval"; } }

        public override void AddToSequence(AnimationSequence animationSequence)
        {
            animationSequence.sequence.AppendInterval(_duration);
        }
    }
}