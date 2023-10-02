using System;

namespace Gespell
{
    [Serializable]
    public struct UnitStat
    {
        public int health;
        public int attack;
        public float interval; // Interval, speed, and range is useless for the player
        public float speed;
        public float range;
        
        public UnitStat(int health, int attack, float interval, float speed, float range)
        {
            this.health = health;
            this.attack = attack;
            this.interval = interval;
            this.speed = speed;
            this.range = range;
        }
    }
}