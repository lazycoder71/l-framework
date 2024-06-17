using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace LFramework
{
    public class AnimationSequenceStepCallback : AnimationSequenceStep
    {
        [SerializeField]
        private bool _isInserted;

        [SerializeField]
        [Min(0f), ShowIf("@_isInserted")]
        private float _insertTime;

        [SerializeField]
        private UnityEvent _callback;

        public override string displayName { get { return "Callback"; } }

        public override void AddToSequence(AnimationSequence animationSequence)
        {
            if (_isInserted)
                animationSequence.sequence.InsertCallback(_insertTime, () => { _callback?.Invoke(); });
            else
                animationSequence.sequence.AppendCallback(() => { _callback?.Invoke(); });
        }
    }
}