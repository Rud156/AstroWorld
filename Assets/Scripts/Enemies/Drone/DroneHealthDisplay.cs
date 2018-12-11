using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AstroWorld.Common;
using AstroWorld.Extras;
using UnityEngine;
using UnityEngine.AI;

namespace AstroWorld.Enemies.Drone
{
    [RequireComponent(typeof(DroneController))]
    [RequireComponent(typeof(HealthSetter))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class DroneHealthDisplay : MonoBehaviour
    {
        [Header("Death")]
        [Range(-20, 0)]
        public float healthThreshold;
        [Range(3, 7)]
        public float maxDestroyTime;
        public GameObject deathExplosion;

        [Header("Drone Damage")]
        public Transform smokeSpawnPoint;
        public GameObject droneDamageSmoke;
        [Range(0, 1)]
        public float thresholdHealthRatio;
        public int minParticles;
        public int maxParticles;

        private Animator _droneAnimator;
        private NavMeshAgent _droneAgent;
        private Rigidbody _droneRB;
        private HealthSetter _healthSetter;
        private DroneController _droneController;

        private List<ParticleSystem> _smokeParticles;
        private bool _droneDead;

        private const string DroneDeadAnimParam = "Dead";

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _droneAnimator = GetComponent<Animator>();
            _droneAgent = GetComponent<NavMeshAgent>();
            _droneRB = GetComponent<Rigidbody>();
            _droneController = GetComponent<DroneController>();

            _smokeParticles = new List<ParticleSystem>();

            _healthSetter = GetComponent<HealthSetter>();
            _healthSetter.healthZero += DroneDead;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update() => SetSmokeBasedOnHealth();

        private void SetSmokeBasedOnHealth()
        {
            float healthRatio = _healthSetter.GetCurrentHealth() / _healthSetter.maxHealthAmount;
            if (healthRatio > thresholdHealthRatio)
                return;

            if (_smokeParticles.Count == 0)
            {
                GameObject smokeInstance = Instantiate(droneDamageSmoke,
                    smokeSpawnPoint.position, droneDamageSmoke.transform.rotation);
                smokeInstance.transform.SetParent(smokeSpawnPoint);

                _smokeParticles = smokeInstance.GetComponentsInChildren<ParticleSystem>().ToList();
            }

            foreach (ParticleSystem particle in _smokeParticles)
            {
                float mappedEmissionRate = ExtensionFunctions.Map(
                    healthRatio,
                    thresholdHealthRatio, 0,
                    minParticles, maxParticles
                );

                ParticleSystem.EmissionModule emission = particle.emission;
                emission.rateOverTime = mappedEmissionRate;
            }
        }

        private void DroneDead()
        {
            if (_droneDead)
                return;

            _droneDead = true;

            _droneAnimator.SetBool(DroneDeadAnimParam, true);
            _droneAgent.enabled = false;
            _healthSetter.enabled = false;
            _droneController.enabled = false;

            _droneRB.isKinematic = false;
            _droneRB.useGravity = true;


            float currentHealth = _healthSetter.GetCurrentHealth();
            float mappedDestroyTime = ExtensionFunctions.Map(
                currentHealth,
                healthThreshold, 0,
                maxDestroyTime, 0
            );

            StartCoroutine(DestroyDrone(mappedDestroyTime));
        }

        private IEnumerator DestroyDrone(float destroyTime)
        {

            yield return new WaitForSeconds(destroyTime);
            Instantiate(deathExplosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}