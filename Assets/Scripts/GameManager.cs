using System.Collections;
using System.Collections.Generic;
using Gespell.Scriptables;
using UnityEngine;

namespace Gespell
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UnitManager unitManager;

        void Start()
        {
            unitManager.SpawnPlayer();
            unitManager.StartNextWave();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
