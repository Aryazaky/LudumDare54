using System;

namespace Gespell
{
    [Serializable]
    public struct UnitStat
    {
        public int health;
        public int attack;
        public float interval; // Interval and speed is useless for the player
        public float speed;
        
        public UnitStat(int health, int attack, float interval, float speed)
        {
            this.health = health;
            this.attack = attack;
            this.interval = interval;
            this.speed = speed;
        }
    }
}