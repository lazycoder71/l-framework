using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework.View
{
    public abstract class ViewTransitionTransform : ViewTransition
    {
        [SerializeField] protected bool _keepDestination = true;

        [HideIf("@_keepDestination")]
        [SerializeField] protected Vector3 _value;

        [SerializeField] protected bool _keepStart = false;

        [HideIf("@_keepStart")]
        [SerializeField] protected Vector3 _valueStart;
    }
}
