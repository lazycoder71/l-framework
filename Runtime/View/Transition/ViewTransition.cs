using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework.View
{
    [Serializable]
    public class ViewTransition
    {
        public virtual string displayName { get; }

        [Range(0.0f, 1.0f)]
        [SerializeField] private float _duration;

        [CustomValueDrawer("GUIDrawInsertTime")]
        [SerializeField] private float _insertTime;

        public void Apply(View view)
        {
            Tween tween = GetTween(view, Mathf.Max(_duration, 0.1f));

            if (_insertTime > 0.0f)
                view.sequence.Insert(_insertTime, tween);
            else
                view.sequence.Join(tween);
        }

        protected virtual Tween GetTween(View popup, float duration)
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
