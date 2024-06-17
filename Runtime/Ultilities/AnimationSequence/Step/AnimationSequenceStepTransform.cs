using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepTransform : AnimationSequenceStepAction<Transform>
    {
        [SerializeField]
        protected Vector3 _value;

        [SerializeField]
        [ShowIf("@_changeStartValue")]
        protected Vector3 _valueStart;
    }
}