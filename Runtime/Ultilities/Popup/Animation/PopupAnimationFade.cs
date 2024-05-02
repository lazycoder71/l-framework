using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class PopupAnimationFade : PopupAnimation
    {
        public override string displayName { get { return "Fade"; } }

        protected override Tween GetTween(Popup popup, float duration)
        {
            CanvasGroup canvasGroup = popup.canvasGroup;

            return canvasGroup.DOFade(1f, duration)
                              .ChangeStartValue(0.0f);
        }
    }
}
