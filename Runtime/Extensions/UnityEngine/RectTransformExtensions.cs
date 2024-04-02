using UnityEngine;

namespace LFramework
{
    public static class RectTransformExtensions
    {
        public static void SetWidth(this RectTransform rectTransform, float width)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public static void SetHeight(this RectTransform rectTransform, float height)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
        }

        public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
        }

        public static void TranslateX(this RectTransform rectTransform, float x)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + x, rectTransform.anchoredPosition.y);
        }

        public static void TranslateY(this RectTransform rectTransform, float y)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + y);
        }

        public static void TranslateXY(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + x, rectTransform.anchoredPosition.y + y);
        }

        public static void AnchorToCorners(this RectTransform rectTransform)
        {
            if (rectTransform.parent == null)
                return;

            RectTransform rectParent = rectTransform.parent.GetComponent<RectTransform>();

            Vector2 newAnchorsMin = new Vector2(rectTransform.anchorMin.x + rectTransform.offsetMin.x / rectParent.rect.width,
                              rectTransform.anchorMin.y + rectTransform.offsetMin.y / rectParent.rect.height);

            Vector2 newAnchorsMax = new Vector2(rectTransform.anchorMax.x + rectTransform.offsetMax.x / rectParent.rect.width,
                              rectTransform.anchorMax.y + rectTransform.offsetMax.y / rectParent.rect.height);

            rectTransform.anchorMin = newAnchorsMin;
            rectTransform.anchorMax = newAnchorsMax;
            rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
        }

        public static float GetPixelPerUnit(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];

            rectTransform.GetWorldCorners(corners);

            return rectTransform.rect.width / (corners[2].x - corners[0].x);
        }

        public static float GetUnitPerPixel(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];

            rectTransform.GetWorldCorners(corners);

            return (corners[2].x - corners[0].x) / rectTransform.rect.width;
        }
    }
}