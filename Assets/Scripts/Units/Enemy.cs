using Gespell.Enums;

namespace Gespell.Units
{
    public class Enemy : UnitBase
    {
        public override void Initialize((UnitStat stat, UnitFaction faction) data)
        {
            base.Initialize((data.stat, UnitFaction.Enemy));
        }
    }
}