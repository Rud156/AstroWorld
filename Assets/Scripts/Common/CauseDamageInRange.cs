using AstroWorld.CustomCamera;
using AstroWorld.Extras;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Common
{
    [RequireComponent(typeof(DamageAmount))]
    public class CauseDamageInRange : MonoBehaviour
    {
        public float damageRange;
        public bool activateOnStart = true;

        [Header("Camera Shake Range")]
        public int maxDistanceForEffect = 100;

        private DamageAmount _damageAmount;
        private Transform _player;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _damageAmount = GetComponent<DamageAmount>();
            _player = GameObject.FindGameObjectWithTag(TagManager.Player)?.transform;

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

            float distanceToPlayer = Vector3.Distance(_player.position, transform.position);
            float normalizedDistance = ExtensionFunctions.Map(
                distanceToPlayer,
                10, Mathf.Min(maxDistanceForEffect, distanceToPlayer),
                0, 1
            );
            CameraShakeManager.instance.ShakeExplosion(normalizedDistance);
        }
    }
}