using System;
using System.Collections.Generic;
using System.Linq;
using Gespell.Interfaces;
using UnityEngine;

namespace Gespell.Scriptables
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Gespell/Wave")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private List<UnitData> enemies = new();
        [SerializeField, Min(1)] private int maxEnemies = 3;
        private readonly List<UnitBase> spawnedEnemies = new();
        [NonSerialized] private bool waveStarted;

        public event Action OnWaveCompletedSpawn;
        public event Action OnWaveCleared;

        public IEnumerable<UnitBase> SpawnedEnemies => spawnedEnemies;

        public void StartWave(UnitManager unitManager, Vector3 spawnOrigin, Vector3 offset, Transform parent)
        {
            if(!waveStarted)
            {
                waveStarted = true;
                spawnedEnemies.Clear();
                for (var index = 0; index < enemies.Count; index++)
                {
                    var unitData = enemies[index];
                    var spawned = unitData.Spawn(unitManager, spawnOrigin + offset * index, parent);
                    spawned.OnDead += EnemyOnDeadHandler;
                    spawned.gameObject.name = $"{unitData.name}_{index}";
                    spawnedEnemies.Add(spawned);
                }

                Debug.Log($"Wave {this} spawn completed");
                OnWaveCompletedSpawn?.Invoke();
            }
            else Debug.LogError($"Wave {this} already started");
        }

        public void StopWave()
        {
            waveStarted = false;
            foreach (var enemy in spawnedEnemies)
            {
                EnemyOnDeadHandler(enemy);
            }
        }

        private void EnemyOnDeadHandler(IDamageable obj)
        {
            obj.OnDead -= EnemyOnDeadHandler;
            spawnedEnemies.Remove(obj as UnitBase);
            if (waveStarted && !spawnedEnemies.Any())
            {
                Debug.Log($"Wave {this} cleared");
                OnWaveCleared?.Invoke();
                waveStarted = false;
            }
        }

        private void OnValidate()
        {
            if (enemies.Count > maxEnemies)
            {
                Debug.LogError($"Enemy count in {this} exceeds the maximum ({enemies.Count}/{maxEnemies})");
                enemies.Remove(enemies.Last());
            }
        }
    }
}