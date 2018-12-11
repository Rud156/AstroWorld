using System.Collections;
using System.Collections.Generic;
using AstroWorld.Spawners;
using UnityEngine;

namespace AstroWorld.Scenes.Main
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager instance;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (instance == null)
                instance = this;

            if (instance != this)
                Destroy(gameObject);
        }

        #endregion Singleton

        public int maxChaosLevel;
        public float maxDronesToSpawn;

        private int _chaosLevel;
        private bool _chaosCreated;

        public void IncrementChaosLevel()
        {
            _chaosLevel += 1;

            if (_chaosLevel > maxChaosLevel && !_chaosCreated)
            {
                _chaosCreated = true;
                SpawnDefenceDrones();
            }
        }

        private IEnumerator SpawnDefenceDrones()
        {
            DroneSpawner.instance.StartSpawn();
            yield return new WaitForSeconds(DroneSpawner.instance.spawnRate * maxDronesToSpawn);
            DroneSpawner.instance.StopSpawn();
        }
    }
}