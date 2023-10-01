using Gespell.Enums;

namespace Gespell.Units
{
    public class Player : UnitBase
    {
        public override void Initialize((UnitStat stat, UnitFaction faction) data)
        {
            base.Initialize((data.stat, UnitFaction.Player));
        }
    }
}