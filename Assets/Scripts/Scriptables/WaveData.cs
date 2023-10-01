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
        private bool waveStarted;

        public event Action OnWaveCompletedSpawn;
        public event Action OnWaveCleared;

        public IEnumerable<UnitBase> SpawnedEnemies => spawnedEnemies;

        public void StartWave(UnitManager unitManager, Vector3 spawnOrigin, Vector3 offset)
        {
            if(!waveStarted)
            {
                waveStarted = true;
                spawnedEnemies.Clear();
                for (var index = 0; index < enemies.Count; index++)
                {
                    var unitData = enemies[index];
                    var spawned = unitData.Spawn(unitManager, spawnOrigin + offset * index);
                    spawned.OnDead += EnemyOnDeadHandler;
                    spawnedEnemies.Add(spawned);
                }
                OnWaveCompletedSpawn?.Invoke();
            }
            else Debug.LogError($"Wave {this} already started");
        }

        private void EnemyOnDeadHandler(IDamageable obj)
        {
            obj.OnDead -= EnemyOnDeadHandler;
            spawnedEnemies.Remove(obj as UnitBase);
            if (waveStarted && !spawnedEnemies.Any())
            {
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