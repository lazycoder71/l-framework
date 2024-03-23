using System;
using UnityEngine;

namespace LFramework
{
    [Serializable]
    public class LValue<T>
    {
        [SerializeField] T _value;

        public T value
        {
            get { return _value; }
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    eventValueChanged?.Invoke(_value);
                }
            }
        }

        public event Action<T> eventValueChanged;

        public LValue(T defaultValue)
        {
            _value = defaultValue;
        }
    }
}