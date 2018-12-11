using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Common
{
    [RequireComponent(typeof(DamageAmount))]
    public class CauseDamageInRange : MonoBehaviour
    {
        public float damageRange;
        public bool activateOnStart = true;

        private DamageAmount _damageAmount;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _damageAmount = GetComponent<DamageAmount>();

            if (activateOnStart)
                DamageInRange();
        }

        public void DamageInRange()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRange);
            foreach (Collider collider in colliders)
            {
                var healthSetter = collider.GetComponent<HealthSetter>();
                if (healthSetter != null)
                    healthSetter.ReduceHealth(_damageAmount.damageAmount);
            }
        }
    }
}