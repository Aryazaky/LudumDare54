using Gespell.Enums;
using UnityEditor.Animations;
using UnityEngine;

namespace Gespell.Scriptables
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Gespell/Unit")]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private UnitBase unitPrefab;
        [SerializeField] private UnitFaction faction;
        [SerializeField] private UnitStat stat;
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private Vector3 healthBarRelativePosition;
        [SerializeField] private HealthBar healthBarPrefab;

        public UnitBase Spawn(UnitManager unitManager, Vector3 position, Transform parent = null)
        {
            var spawned = Instantiate(unitPrefab, position, Quaternion.identity, parent);
            spawned.Initialize((unitManager, animatorController, stat, faction));
            if (healthBarPrefab != null)
            {
                var healthBar = Instantiate(healthBarPrefab, spawned.transform);
                healthBar.transform.localPosition = healthBarRelativePosition;
                healthBar.Initialize(spawned);
            }
            return spawned;
        }
    }
}