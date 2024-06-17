using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepRectTransform : AnimationSequenceStepAction<RectTransform>
    {
        [SerializeField]
        protected Vector3 _value;

        [SerializeField]
        [ShowIf("@_changeStartValue")]
        protected Vector3 _valueStart;

        [SerializeField] 
        protected bool _snapping = false;
    }
}