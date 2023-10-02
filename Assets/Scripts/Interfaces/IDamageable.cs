using System;

namespace Gespell.Interfaces
{
    public interface IDamageable
    {
        bool IsDead { get; }
        void Damage(int amount);
        event Action<IDamageable> OnDamaged;
        event Action<IDamageable> OnDead;
    }
}