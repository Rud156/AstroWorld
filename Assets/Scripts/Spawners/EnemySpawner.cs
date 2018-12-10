using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemy;
        public float initialSpawnCount;
        public Transform[] spawningPoints;

        [Header("Debug")]
        public bool spawnOnStart;

        private Transform _enemyHolder;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _enemyHolder = GameObject.FindGameObjectWithTag(TagManager.CreatureHolder)?.transform;

            if (spawnOnStart)
                StartSpawn();
        }

        private void StartSpawn()
        {
            for (int i = 0; i < initialSpawnCount; i++)
            {
                int randomIndex = Random.Range(0, 1000) % spawningPoints.Length;
                Vector3 spawnPoint = spawningPoints[randomIndex].position;

                GameObject enemyInstance = Instantiate(enemy, spawnPoint, enemy.transform.rotation);
                enemyInstance.transform.SetParent(_enemyHolder);
            }
        }
    }
}