using System;
using UnityEngine;

namespace LFramework
{
    [Serializable]
    public class LValue<T>
    {
        [SerializeField] T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                EventValueChanged?.Invoke(_value);
            }
        }

        public event Action<T> EventValueChanged;

        public LValue(T defaultValue)
        {
            _value = defaultValue;
        }
    }
}