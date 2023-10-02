using System;
using Gespell.Interfaces;
using UnityEngine;

namespace Gespell
{
    public abstract class HealthBar : MonoBehaviour, IInitializable<IHasHealthBar>
    {
        protected IHasHealthBar Owner;

        private void OnEnable()
        {
            if (Owner != null) Owner.OnHealthChanged += OwnerOnHealthChanged;
        }

        private void OnDisable()
        {
            if (Owner != null) Owner.OnHealthChanged -= OwnerOnHealthChanged;
        }

        public bool Initialized { get; private set; }
        
        public void Initialize(IHasHealthBar data)
        {
            if(data == null)
            {
                Debug.LogError($"IHasHealthBar is null! Cannot initialize {this}");
                return;
            }
            Owner = data;
            Owner.OnHealthChanged += OwnerOnHealthChanged;
            Initialized = true;
        }

        protected abstract void OwnerOnHealthChanged(int value);
    }
}