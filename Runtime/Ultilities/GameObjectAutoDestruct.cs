using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class GameObjectAutoDestruct : MonoCached
    {
        [Title("Config")]
        [SerializeField] float _delay = 0f;
        [SerializeField] bool _deactiveOnly = false;

        Tween _tween;

        public event Action<GameObject> eventDestruct;

        #region MonoBehaviour

        private void OnEnable()
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(_delay, Destruct, false);
        }

        private void OnDisable()
        {
            eventDestruct?.Invoke(gameObjectCached);
			
            _tween?.Kill();
        }

        #endregion

        private void Destruct()
        {
            if (_deactiveOnly)
                gameObjectCached.SetActive(false);
            else
                Destroy(gameObjectCached);
        }
    }
}