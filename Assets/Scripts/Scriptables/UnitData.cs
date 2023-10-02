using Gespell.Enums;
using UnityEngine;

namespace Gespell.Scriptables
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Gespell/Unit")]
    public class UnitData : UnitDataBase
    {
        [SerializeField] private UnitFaction faction;
        [SerializeField] private UnitStat stat;

        public override UnitBase Spawn(UnitManager unitManager, Vector3 position, Transform parent = null)
        {
            if (unitPrefab == null) throw new MissingReferenceException($"Unit prefab is null in {this}");
            var spawned = Instantiate(unitPrefab, position, Quaternion.identity, parent);
            InitializeUnit(spawned, unitManager, stat, faction);
            return spawned;
        }
    }
}