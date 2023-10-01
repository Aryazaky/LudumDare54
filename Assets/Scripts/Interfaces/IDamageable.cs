﻿using System;
using UnityEngine;

namespace Gespell.Interfaces
{
    public interface IDamageable
    {
        bool IsDead { get; }
        void Damage(int amount);
        event Action<IDamageable> OnDead;
    }
}