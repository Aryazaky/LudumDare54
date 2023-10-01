using System;
using System.Collections.Generic;
using System.Linq;
using Gespell.Scriptables;
using UnityEngine;

namespace Gespell
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private UnitData playerData;
        [SerializeField] private Vector3 playerPosition;
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
                    currentWave.StartWave(this, waveSpawnOrigin, waveSpawnOffset);
                }
            }
        }

        private void OnCurrentWaveClearedHandler()
        {
            currentWave.OnWaveCleared -= OnCurrentWaveClearedHandler;
            currentWave = null;
            waveIndex++;
            if(waveIndex >= 0 && waveIndex < waves.Count)
            {
                StartNextWave();
            }
            else OnAllWaveCleared?.Invoke();
        }
    }
}