using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class LCollectDestination : MonoBase
    {
        [Title("Reference")]
        [SerializeField] Transform _target;

        [Title("Config")]
        [SerializeField] LCollectConfig _config;

        public event Action<int> eventReturnBegin;
        public event Action<float> eventReturn;
        public event Action eventReturnEnd;

        int _returnExpect;
        int _returnCount;

        public LCollectConfig config { get { return _config; } }

        public Vector3 position { get { return _target == null ? TransformCached.position : _target.position; } }

        protected override void OnEnable()
        {
            base.OnEnable();

            LCollectDestinationHelper.Push(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            LCollectDestinationHelper.Pop(this);
        }

        public void ReturnBegin(int valueCount, int spawnCount)
        {
            _returnExpect += spawnCount;

            eventReturnBegin?.Invoke(valueCount);
        }

        public void Return()
        {
            _returnCount++;

            if (_returnCount == _returnExpect)
            {
                _returnCount = 0;
                _returnExpect = 0;

                eventReturn?.Invoke(1.0f);
            }
            else
            {
                eventReturn?.Invoke((float)_returnCount / _returnExpect);
            }
        }

        public void ReturnEnd()
        {
            eventReturnEnd?.Invoke();
        }
    }
}