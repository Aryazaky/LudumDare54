using Gespell.Enums;

namespace Gespell.Units
{
    public class Player : UnitBase
    {
        public override void Initialize((UnitManager unitManager, UnitStat stat, UnitFaction faction) data)
        {
            base.Initialize((data.unitManager, data.stat, UnitFaction.Player));
        }
    }
}