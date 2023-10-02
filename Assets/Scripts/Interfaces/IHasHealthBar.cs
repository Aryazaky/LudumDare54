using System;

namespace Gespell.Interfaces
{
    public interface IHasHealthBar
    {
        int MaxHealth { get; }
        event Action<int> OnHealthChanged;
    }
}