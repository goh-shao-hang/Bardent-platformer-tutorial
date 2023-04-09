using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.CoreSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        [field: SerializeField] public float MaxValue { get; private set; }

        private float _currentValue;
        public float CurrentValue
        {
            get => _currentValue;
            private set
            {
                _currentValue = Mathf.Clamp(value, 0, MaxValue);
                if (_currentValue <= 0f)
                {
                    OnCurrentValueZero?.Invoke();
                }
            }
        }

        public Stat Init()
        {
            _currentValue = MaxValue;
            return this;
        }

        public void Increase(float amount) => CurrentValue += amount;

        public void Decrease(float amount) => CurrentValue -= amount;
    }
}
