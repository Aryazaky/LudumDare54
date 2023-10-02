using System;
using Gespell.Enums;
using UnityEngine;

namespace Gespell.Scriptables
{
    public abstract class UnitDataBase : ScriptableObject
    {
        [SerializeField] protected UnitBase unitPrefab;
        [SerializeField] protected RuntimeAnimatorController animatorController;
        [SerializeField] protected Vector3 healthBarRelativePosition;
        [SerializeField] protected HealthBar healthBarPrefab;

        protected virtual void InitializeUnit(UnitBase spawned, UnitManager unitManager, UnitStat stat, UnitFaction faction)
        {
            if (spawned == null) throw new ArgumentNullException(nameof(spawned), "Spawned unit is null");
            if (unitPrefab == null) throw new MissingReferenceException($"Unit prefab is null in {this}");

            spawned.Initialize((unitManager, animatorController, stat, faction));

            if (healthBarPrefab != null)
            {
                var healthBar = Instantiate(healthBarPrefab, spawned.transform);
                healthBar.transform.localPosition = healthBarRelativePosition;
                healthBar.Initialize(spawned);
            }
        }

        protected virtual void OnValidate()
        {
            if (unitPrefab == null) Debug.LogError($"Unit prefab is null in {this}");
            if (healthBarPrefab == null) Debug.LogWarning($"Health bar prefab is null in {this}");
        }

        public abstract UnitBase Spawn(UnitManager unitManager, Vector3 position, Transform parent = null);
    }
}