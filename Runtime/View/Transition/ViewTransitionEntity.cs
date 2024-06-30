using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework.View
{
    public class ViewTransitionEntity : MonoCached
    {
        [Title("Config")]
        [Range(0.1f, 1f)]
        [SerializeField] private float _duration = 1f;

        [CustomValueDrawer("GUIDrawInsertTime")]
        [SerializeField] private float _insertTime = 0f;

        [SerializeField] protected Ease _ease;

        public void Apply(View view)
        {
            Tween tween = GetTween(view, _duration);

            if (_insertTime > 0.0f)
                view.sequence.Insert(_insertTime, tween);
            else
                view.sequence.Append(GetTween(view, _duration));
        }

        protected virtual Tween GetTween(View view, float duration)
        {
            return null;
        }

#if UNITY_EDITOR

        private float GUIDrawInsertTime(float insertTime, GUIContent label)
        {
            return UnityEditor.EditorGUILayout.Slider(label, insertTime, 0f, 1.0f - _duration);
        }

#endif
    }
}
