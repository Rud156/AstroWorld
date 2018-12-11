using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Spawners
{
    public class CreatureSpawner : MonoBehaviour
    {
        #region Singleton

        public static CreatureSpawner instance;

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

        public GameObject enemy;
        public int initialSpawnCount;
        public Transform[] spawningPoints;

        [Header("Debug")]
        public bool spawnOnStart;

        private Transform _creatureHolder;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _creatureHolder = GameObject.FindGameObjectWithTag(TagManager.CreatureHolder)?.transform;

            if (spawnOnStart)
                StartSpawn();
        }

        private void StartSpawn()
        {
            for (int i = 0; i < initialSpawnCount; i++)
            {
                int randomIndex = Random.Range(0, 1000) % spawningPoints.Length;
                Vector3 spawnPoint = spawningPoints[randomIndex].position;

                GameObject creatureInstance = Instantiate(enemy, spawnPoint, enemy.transform.rotation);
                creatureInstance.transform.SetParent(_creatureHolder);
            }
        }
    }
}