using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LFramework
{
    public class ViewExtraBackground : ViewExtra
    {
        [SerializeField] bool _closeOnClick = true;
        [SerializeField] Sprite _sprite;
        [SerializeField] Color _color = new Color(0f, 0f, 0f, 0.8f);

        public override string displayName { get { return "Background"; } }

        GameObject _objBG;

        protected override Tween GetTween(View view, float duration)
        {
            view.onCloseEnd.AddListener(() =>
            {
                Object.Destroy(_objBG);
            });

            // Spawn background
            SpawnBackground(view);

            // Return background fade tween
            Image image = _objBG.GetComponent<Image>();
            image.color = _color;
            image.sprite = _sprite;

            return image.DOFade(_color.a, duration)
                        .ChangeStartValue(new Color(_color.r, _color.g, _color.b, 0.0f))
                        .SetEase(Ease.Linear);
        }

        void SpawnBackground(View view)
        {
            _objBG = new GameObject($"{view.name} Background");

            RectTransform rect = _objBG.AddComponent<RectTransform>();
            rect.SetParent(view.transformCached.parent);
            rect.SetSiblingIndex(view.transform.GetSiblingIndex());
            rect.SetScale(1.0f);

            rect.StretchByParent();

            _objBG.AddComponent<Image>();

            if (_closeOnClick)
            {
                Button button = _objBG.AddComponent<Button>();
                button.transition = Selectable.Transition.None;
                button.onClick.AddListener(view.Close);
            }
        }
    }
}
