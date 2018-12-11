using System.Collections;
using System.Collections.Generic;
using AstroWorld.Common;
using UnityEngine;

namespace AstroWorld.Enemies.Creature
{
    [RequireComponent(typeof(HealthSetter))]
    public class AlertNearbyEnemiesOnDead : MonoBehaviour
    {
        public GameObject battery;
        public float enemyAlertRange;

        private HealthSetter _healthSetter;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _healthSetter = GetComponent<HealthSetter>();
            _healthSetter.healthZero += OnEnemyDead;
        }

        private void OnEnemyDead()
        {
            Instantiate(battery, transform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyAlertRange);
            foreach (Collider collider in colliders)
            {
                CreaturePatrol creaturePatrol = collider.GetComponent<CreaturePatrol>();
                if (creaturePatrol != null)
                    creaturePatrol.SetPlayerHostile();
            }
        }
    }
}