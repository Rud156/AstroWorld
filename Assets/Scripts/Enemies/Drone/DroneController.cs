using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace AstroWorld.Enemies.Drone
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(DroneAttack))]
    [RequireComponent(typeof(Animator))]
    public class DroneController : MonoBehaviour
    {
        [Header("Velocities")]
        public float lookingSpeed;
        public float movementThreshold;

        private NavMeshAgent _droneAgent;
        private DroneAttack _droneAttack;

        private bool _droneAttacking;
        private Animator _droneAnimator;
        private Transform _player;

        private const string DroneMovementAnimParam = "Movement";

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _droneAgent = GetComponent<NavMeshAgent>();
            _droneAttack = GetComponent<DroneAttack>();

            _player = GameObject.FindGameObjectWithTag(TagManager.Player)?.transform;
            _droneAnimator = GetComponent<Animator>();

            _droneAttacking = false;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (!_droneAgent.isOnNavMesh)
                return;

            SetDestinationPlayer();
            LookTowardsTarget();

            UpdateDroneAnimation();

            if (!_droneAgent.pathPending && !_droneAttacking)
            {
                if (_droneAgent.remainingDistance <= _droneAgent.stoppingDistance)
                {
                    if (!_droneAgent.hasPath || _droneAgent.velocity.sqrMagnitude == 0f)
                        StartCoroutine(AttackPlayer());
                }
            }
        }

        private void SetDestinationPlayer()
        {
            if (_player != null)
                _droneAgent.SetDestination(_player.position);
        }

        private void UpdateDroneAnimation()
        {
            if (_droneAnimator.velocity.magnitude > movementThreshold)
                _droneAnimator.SetBool(DroneMovementAnimParam, true);
            else
                _droneAnimator.SetBool(DroneMovementAnimParam, false);
        }

        private void LookTowardsTarget()
        {
            if (_player == null)
                return;

            Vector3 lookDirection = _player.position - transform.position;
            lookDirection.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                lookingSpeed * Time.deltaTime);
        }

        private IEnumerator AttackPlayer()
        {
            _droneAttacking = true;

            float attackTime = _droneAttack.AttackTarget(_player);
            yield return new WaitForSeconds(attackTime);

            _droneAttacking = false;
        }
    }
}