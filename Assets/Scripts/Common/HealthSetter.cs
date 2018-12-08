﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Common
{
    [RequireComponent(typeof(Collider))]
    public class HealthSetter : MonoBehaviour
    {
        public delegate void HealthZero();
        public HealthZero healthZero;

        public float maxHealthAmount;
        public GameObject deathEffect;
        public Transform effectInstantiatePoint;
        public bool destoryOnZero = true;

        private float _currentHealthAmount;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start() => _currentHealthAmount = maxHealthAmount;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update() => CheckIfHealthZero();

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            DamageAmount damageAmountSetter = other.GetComponent<DamageAmount>();
            if (damageAmountSetter != null)
            {
                Destroy(other.gameObject);
                ReduceHealth(damageAmountSetter.damageAmount);
            }
        }

        public float GetCurrentHealth() => _currentHealthAmount;

        public void AddHealth(float healthAmount) => _currentHealthAmount =
            _currentHealthAmount + healthAmount > maxHealthAmount ?
                maxHealthAmount : _currentHealthAmount + healthAmount;

        public void ReduceHealth(float healthAmount) => _currentHealthAmount -= healthAmount;

        private void CheckIfHealthZero()
        {
            if (_currentHealthAmount <= 0)
            {
                SpawnDestroyEffect();

                healthZero?.Invoke();
                if (destoryOnZero)
                    Destroy(gameObject);
            }
        }

        private void SpawnDestroyEffect()
        {
            GameObject particleEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            particleEffect.transform.position = effectInstantiatePoint.position;
        }
    }
}