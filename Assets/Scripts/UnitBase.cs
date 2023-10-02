using System;
using Gespell.Enums;
using Gespell.Interfaces;
using UnityEngine;

namespace Gespell
{
    public abstract class UnitBase : MonoBehaviour, IHasStats, IHasFaction, IDamageable, ICanAttack, IHealable, IHasHealthBar,
        IInitializable<(UnitManager unitManager, UnitStat stat, UnitFaction faction)>
    {
        protected UnitManager Manager;
        private UnitStat stat;
        public new Transform transform { get; private set; }
        public UnitStat Stat => stat;
        public UnitFaction Faction { get; private set; }
        public bool IsDead { get; private set; }
        public bool Initialized { get; private set; }

        private void Awake()
        {
            transform = base.transform;
        }

        public virtual void Initialize((UnitManager unitManager, UnitStat stat, UnitFaction faction) data)
        {
            Manager = data.unitManager;
            stat = data.stat;
            MaxHealth = data.stat.health;
            Faction = data.faction;
            Initialized = true;
        }
        
        public virtual void Attack(IDamageable target, int amount)
        {
            Debug.Log($"{this} attacks {target}");
            target.Damage(amount);
        }
        
        public virtual void Damage(int amount)
        {
            if (!IsDead)
            {
                Debug.Log($"{this} take {amount} damage");
                stat.health -= amount;
                OnDamaged?.Invoke(this);
                OnHealthChanged?.Invoke(stat.health);
                if (stat.health <= 0)
                {
                    stat.health = 0;
                    Debug.Log($"{this} dead");
                    IsDead = true;
                    OnDead?.Invoke(this);
                }
            }
        }

        public event Action<IDamageable> OnDamaged;

        public event Action<IDamageable> OnDead;
        
        public void Heal(int amount)
        {
            if (!IsDead)
            {
                Debug.Log($"{this} healed by {amount}");
                stat.health += amount;
                OnHealed?.Invoke(this);
                OnHealthChanged?.Invoke(stat.health);
            }
        }

        public event Action<IDamageable> OnHealed;
        
        public int MaxHealth { get; private set; }
        public event Action<int> OnHealthChanged;
    }
}