using System.Collections;
using System.Collections.Generic;
using AstroWorld.Common;
using UnityEngine;

namespace AstroWorld.Enemies.Creature
{
    [RequireComponent(typeof(HealthSetter))]
    [RequireComponent(typeof(DamageAmount))]
    public class AlertNearbyEnemiesOnDead : MonoBehaviour
    {
        public GameObject battery;
        public float enemyRangeToTarget;

        private DamageAmount _damageAmount;
        private HealthSetter _healthSetter;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _damageAmount = GetComponent<DamageAmount>();

            _healthSetter = GetComponent<HealthSetter>();
            _healthSetter.healthZero += OnEnemyDead;
        }

        private void OnEnemyDead()
        {
            Instantiate(battery, transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyRangeToTarget);

            foreach (Collider collider in colliders)
            {
                HealthSetter healthSetter = collider.GetComponent<HealthSetter>();
                if (healthSetter != null)
                    healthSetter.ReduceHealth(_damageAmount.damageAmount);

                CreaturePatrol creaturePatrol = collider.GetComponent<CreaturePatrol>();
                if (creaturePatrol != null)
                    creaturePatrol.SetPlayerHostile();
            }
        }
    }
}