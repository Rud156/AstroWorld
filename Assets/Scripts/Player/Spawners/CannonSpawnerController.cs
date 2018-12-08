﻿using AstroWorld.Extras;
using AstroWorld.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Player.Spawners
{
    public class CannonSpawnerController : MonoBehaviour
    {
        [Header("Main Object")]
        public GameObject laserCannon;

        [Header("Stats")]
        [Range(3, 10)]
        public float spawnBeforeDistance;
        public float collectionTime;

        private bool _cannonInRange;
        private float _currentCollectionTime;
        private bool _cannonSpawned;

        private Image _displayImage;
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
                _displayImage.enabled = false;
            }
        }

        private void CheckDeployAndCannon()
        {
            if (_cannonSpawned)
                return;

            if (Input.GetKeyDown(Controls.LaserSpawnKey))
            {
                if (_cannonInstance == null)
                {
                    _cannonInstance = Instantiate(
                         laserCannon,
                         transform.position + transform.forward * spawnBeforeDistance,
                         laserCannon.transform.rotation
                     );
                    _displayImage = _cannonInstance.GetComponentInChildren<Image>();
                }
                else
                {
                    _cannonInstance.SetActive(true);
                    _cannonInstance.transform.position = transform.position +
                        transform.forward * spawnBeforeDistance;
                }

                _cannonSpawned = true;
                _displayImage.enabled = false;
            }
        }

        private void CheckAndPickUpCannon()
        {
            if (!_cannonSpawned)
                return;

            if (Input.GetKey(Controls.LaserSpawnKey) && _cannonInRange)
                _currentCollectionTime -= Time.deltaTime;

            _displayImage.enabled = true;
            _displayImage.fillAmount = ExtensionFunctions.Map(_currentCollectionTime, 0, collectionTime,
                0, 1);

            if (_currentCollectionTime <= 0)
            {
                _cannonInstance.SetActive(false);
                _cannonSpawned = false;
                _currentCollectionTime = collectionTime;
                _displayImage.enabled = false;
            }
        }
    }
}