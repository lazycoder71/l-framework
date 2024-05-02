using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LFramework
{
    public class PopupAnimationBackground : PopupAnimation
    {
        [SerializeField] bool _closeOnClick = true;
        [SerializeField] Color _color = new Color(0f, 0f, 0f, 0.5f);

        public override string displayName { get { return "Background"; } }

        GameObject _objBG;

        protected override Tween GetTween(Popup popup, float duration)
        {
            popup.onCloseEnd.AddListener(() =>
            {
                if (popup.deactiveOnClosed)
                    _objBG.SetActive(false);
                else
                    Object.Destroy(_objBG);
            });

            popup.onOpenStart.AddListener(() =>
            {
                if (popup.deactiveOnClosed)
                    _objBG.SetActive(true);
            });

            // Spawn background
            SpawnBackground(popup);

            // Return background fade tween
            Image image = _objBG.GetComponent<Image>();
            image.color = _color;

            return image.DOFade(_color.a, duration)
                        .ChangeStartValue(new Color(_color.r, _color.g, _color.b, 0f))
                        .SetEase(Ease.Linear);
        }

        void SpawnBackground(Popup popup)
        {
            _objBG = new GameObject($"{popup.name} Background");

            RectTransform rect = _objBG.AddComponent<RectTransform>();
            rect.SetParent(popup.transformCached.parent);
            rect.SetSiblingIndex(popup.transform.GetSiblingIndex());
            rect.SetScale(1.0f);

            rect.StretchByParent();

            _objBG.AddComponent<Image>();

            if (_closeOnClick)
            {
                Button button = _objBG.AddComponent<Button>();
                button.transition = Selectable.Transition.None;
                button.onClick.AddListener(popup.Close);
            }
        }
    }
}
