using Gespell.Enums;
using UnityEngine;

namespace Gespell.Scriptables
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Gespell/Unit")]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private UnitBase unitPrefab;
        [SerializeField] private UnitFaction faction;
        [SerializeField] private UnitStat stat;

        public UnitBase Spawn(Vector3 position, Transform parent = null)
        {
            var spawned = Instantiate(unitPrefab, position, Quaternion.identity, parent);
            spawned.Initialize((stat, faction));
            return spawned;
        }
    }
}