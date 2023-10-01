using System;
using System.Collections.Generic;
using System.Linq;
using Gespell.Scriptables;
using Gespell.Utilities;
using UnityEngine;

namespace Gespell
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private UnitData playerData;
        [SerializeField] private Vector3 playerPosition;
        [SerializeField] private LinearObjectArranger enemyParent;
        [SerializeField] private Vector3 waveSpawnOrigin;
        [SerializeField] private Vector3 waveSpawnOffset;
        [SerializeField] private List<WaveData> waves = new();
        private int waveIndex;
        private WaveData currentWave;

        public event Action OnAllWaveCleared;

        public UnitBase GetSpawnedEnemy()
        {
            return currentWave != null ? currentWave.SpawnedEnemies.First() : null;
        }

        public UnitBase Player { get; private set; }

        public void SpawnPlayer()
        {
            Player = playerData.Spawn(this, playerPosition, transform);
        }

        public void StartNextWave()
        {
            if (currentWave == null)
            {
                if (waveIndex >= 0 && waveIndex < waves.Count)
                {
                    currentWave = waves[waveIndex];
                    currentWave.OnWaveCleared += OnCurrentWaveClearedHandler;
                    currentWave.OnWaveCompletedSpawn += enemyParent.ArrangeChildren;
                    currentWave.StartWave(this, waveSpawnOrigin, waveSpawnOffset, enemyParent.transform);
                }
            }
        }

        private void OnCurrentWaveClearedHandler()
        {
            Unsubscribe();
            currentWave = null;
            waveIndex++;
            if(waveIndex >= 0 && waveIndex < waves.Count)
            {
                StartNextWave();
            }
            else OnAllWaveCleared?.Invoke();
        }

        private void Unsubscribe()
        {
            currentWave.OnWaveCleared -= OnCurrentWaveClearedHandler;
            currentWave.OnWaveCompletedSpawn -= enemyParent.ArrangeChildren;
        }

        private void OnValidate()
        {
            if(waves.Contains(null)) Debug.LogError($"There is a null entry in {this}!");
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            if (currentWave != null)
            {
                Unsubscribe();
            }
        }
    }
}