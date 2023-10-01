using System;

namespace Gespell
{
    [Serializable]
    public struct UnitStat
    {
        public int health;
        public int attack;
        public float interval;
        
        public UnitStat(int health, int attack, float interval)
        {
            this.health = health;
            this.attack = attack;
            this.interval = interval;
        }
    }
}