using Gespell.Enums;
using UnityEditor.Animations;
using UnityEngine;

namespace Gespell.Units
{
    public class Player : UnitBase
    {
        public override void Initialize((UnitManager unitManager, AnimatorController animatorController, UnitStat stat, UnitFaction faction) data)
        {
            base.Initialize((data.unitManager, data.animatorController, data.stat, UnitFaction.Player));
        }

        protected override void OnDeadVirtual()
        {
            base.OnDeadVirtual();
            Debug.Log("Game Over");
        }
    }
}