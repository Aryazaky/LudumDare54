using System;

namespace Gespell.Interfaces
{
    public interface IHealable
    {
        void Heal(int amount);
        event Action<IDamageable> OnHealed;
    }
}