using System.Collections.Generic;
using Gespell.Scriptables;
using UnityEngine;

namespace Gespell
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private List<WaveData> enemyWaves = new();
    }
}