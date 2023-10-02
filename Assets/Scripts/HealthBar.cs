using System;
using Gespell.Interfaces;
using UnityEngine;

namespace Gespell
{
    public abstract class HealthBar : MonoBehaviour, IInitializable<IHasHealthBar>
    {
        protected IHasHealthBar unit;

        private void OnEnable()
        {
            if (unit != null) unit.OnHealthChanged += UnitOnHealthChanged;
        }

        private void OnDisable()
        {
            if (unit != null) unit.OnHealthChanged -= UnitOnHealthChanged;
        }

        public bool Initialized { get; private set; }
        
        public void Initialize(IHasHealthBar data)
        {
            if(data == null)
            {
                Debug.LogError($"IHasHealthBar is null! Cannot initialize {this}");
                return;
            }
            unit = data;
            unit.OnHealthChanged += UnitOnHealthChanged;
            Initialized = true;
        }

        protected abstract void UnitOnHealthChanged(int value);
    }
}