using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Spawners
{
    public class DroneSpawner : MonoBehaviour
    {
        #region Singleton

        public static DroneSpawner instance;

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

        public GameObject drone;
        public float spawnRate;
        public Transform[] spawnPoints;

        [Header("Debug")]
        public bool spawnOnStart;

        private Transform _droneHolder;
        private Coroutine _coroutine;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _droneHolder = GameObject.FindGameObjectWithTag(TagManager.DroneHolder)?.transform;

            if (spawnOnStart)
                StartSpawn();
        }

        public void StartSpawn() => _coroutine = StartCoroutine(SpawnDrones());

        public void StopSpawn() => StopCoroutine(_coroutine);

        private IEnumerator SpawnDrones()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnRate);

                int randomIndex = Random.Range(0, 1000) % spawnPoints.Length;
                Vector3 spawnPoint = spawnPoints[randomIndex].position;

                GameObject droneInstance = Instantiate(drone, spawnPoint, drone.transform.rotation);
                droneInstance.transform.SetParent(_droneHolder);
            }
        }
    }
}