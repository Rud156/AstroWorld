using System;
using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Spawners
{
    public class CannonSpawnerController : MonoBehaviour
    {
        public GameObject laserCannon;
        [Range(3, 10)]
        public float spawnBeforeDistance;
        public float collectionTime;

        private bool _cannonInRange;
        private float _currentCollectionTime;
        private bool _cannonSpawned;

        private GameObject _cannonInstance;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            CheckDeployAndCannon();
            CheckAndPickUpCannon();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagManager.Cannon))
                _cannonInRange = true;

        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagManager.Cannon))
            {
                _cannonInRange = false;
                _currentCollectionTime = collectionTime;
            }
        }

        private void CheckDeployAndCannon()
        {
            if (_cannonSpawned)
                return;

            if (Input.GetKeyDown(Controls.LaserSpawnKey))
            {
                if (_cannonInstance == null)
                    _cannonInstance = Instantiate(
                         laserCannon,
                         transform.position + transform.forward * spawnBeforeDistance,
                         laserCannon.transform.rotation
                     );
                else
                {
                    _cannonInstance.SetActive(true);
                    _cannonInstance.transform.position = transform.position +
                        transform.forward * spawnBeforeDistance;
                }

                _cannonSpawned = true;
            }
        }

        private void CheckAndPickUpCannon()
        {
            if (!_cannonSpawned)
                return;

            if (Input.GetKey(Controls.LaserSpawnKey) && _cannonInRange)
                _currentCollectionTime -= Time.deltaTime;

            if (_currentCollectionTime <= 0)
            {
                _cannonInstance.SetActive(false);
                _cannonSpawned = false;
                _currentCollectionTime = collectionTime;
            }
        }
    }
}