using DG.Tweening;
using System;
using UnityEngine;

namespace LFramework
{
    public class AutoDestructObject : MonoCached
    {
        [Header("Config")]
        [SerializeField] float _delay = 0f;
        [SerializeField] bool _deactiveOnly = false;

        Tween _tween;

        public event Action eventDestruct;

        #region MonoBehaviour

        private void OnEnable()
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(_delay, Destruct);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        #endregion

        private void Destruct()
        {
            if (_deactiveOnly)
                gameObjectCached.SetActive(false);
            else
                Destroy(gameObjectCached);

            eventDestruct?.Invoke();
        }
    }
}