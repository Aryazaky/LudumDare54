using System;
using Gespell.Enums;
using Gespell.Interfaces;
using UnityEngine;

namespace Gespell
{
    public abstract class UnitBase : MonoBehaviour, IHasStats, IHasFaction, IDamageable, ICanAttack,
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
            Faction = data.faction;
            Initialized = true;
        }
        
        public virtual void Attack(IDamageable target, int amount)
        {
            target.Damage(amount);
        }
        
        public virtual void Damage(int amount)
        {
            stat.health -= amount;
            if (stat.health <= 0)
            {
                stat.health = 0;
                if (!IsDead)
                {
                    IsDead = true;
                    OnDead?.Invoke(this);
                }
            }
        }

        public event Action<IDamageable> OnDead;
    }
}