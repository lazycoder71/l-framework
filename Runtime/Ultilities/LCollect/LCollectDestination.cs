using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class LCollectDestination : MonoCached
    {
        [Title("Reference")]
        [SerializeField] Transform _target;

        [Title("Config")]
        [SerializeField] LCollectConfig _config;

        public event Action<int> eventReturnBegin;
        public event Action<float> eventReturn;

        int _returnExpect;
        int _returnCount;

        public LCollectConfig config { get { return _config; } }

        public Vector3 position { get { return _target == null ? transformCached.position : _target.position; } }

        private void OnEnable()
        {
            LCollectDestinationHelper.Push(this);
        }

        private void OnDestroy()
        {
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
    }
}