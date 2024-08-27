using UnityEngine;

namespace LFramework
{
    public class MonoBase : MonoBehaviour
    {
        private GameObject _gameObject;

        private Transform _transform;

        public Transform transformCached
        {
            get
            {
                if (_transform == null)
                    _transform = transform;

                return _transform;
            }
        }

        public GameObject gameObjectCached
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = gameObject;

                return _gameObject;
            }
        }

        protected virtual void OnEnable()
        {
            MonoCallback.instance.eventUpdate += Tick;
            MonoCallback.instance.eventLateUpdate += LateTick;
            MonoCallback.instance.eventFixedUpdate += FixedTick;
        }

        protected virtual void OnDisable()
        {
            MonoCallback.instance.eventUpdate -= Tick;
            MonoCallback.instance.eventLateUpdate -= LateTick;
            MonoCallback.instance.eventFixedUpdate -= FixedTick;
        }

        protected virtual void Tick()
        {
        }

        protected virtual void LateTick()
        {
        }

        protected virtual void FixedTick()
        {
        }
    }
}