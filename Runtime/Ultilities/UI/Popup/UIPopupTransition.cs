using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public abstract class UIPopupTransition : MonoBehaviour
    {
        public abstract Tween ConstructTransition(UIPopupBehaviour popup);
    }
}