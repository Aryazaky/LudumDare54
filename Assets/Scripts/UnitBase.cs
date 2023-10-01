using System;
using Gespell.Enums;
using Gespell.Interfaces;
using UnityEngine;

namespace Gespell
{
    public class UnitBase : MonoBehaviour, IInitializable<(UnitStat stat, UnitFaction faction)>, IHasStats, IHasFaction,
        IDamageable, ICanAttack
    {
        private UnitStat stat;
        public UnitStat Stat => stat;
        public UnitFaction Faction { get; private set; }
        public bool IsDead { get; private set; }
        public bool Initialized { get; private set; }
        
        public void Initialize((UnitStat stat, UnitFaction faction) data)
        {
            stat = data.stat;
            Faction = data.faction;
            Initialized = true;
        }
        
        public void Attack(IDamageable target, int amount)
        {
            target.Damage(amount);
        }
        
        public void Damage(int amount)
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