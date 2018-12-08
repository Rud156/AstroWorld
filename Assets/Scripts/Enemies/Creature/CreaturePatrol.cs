using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AstroWorld.Enemies.Creature
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class CreaturePatrol : MonoBehaviour
    {
        private enum CreatureState
        {
            PlayerFound,
            PlayerHostile,
            Idle
        };

        [Header("Distances")]
        public float stopFromPlayer;
        public float stopFromPoint;
        public float maximumDetectionDistance;

        [Header("Velocities")]
        public float lookRotationSpeed;

        [Header("NavMesh")]
        public float maxHeight;

        [Header("Times")]
        public float lazingTime;

        private NavMeshAgent _creatureAgent;
        private Vector3[] _vertices;

        private Transform _currentTarget;
        private Transform _player;

        private CreatureState _creatureState;
        private Vector3 _targetVertex;

        private Coroutine _coroutine;

        private bool _lazingAround;
        private bool _attacking;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _creatureAgent = GetComponent<NavMeshAgent>();

            _player = GameObject.FindGameObjectWithTag("Player")?.transform;

            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            _vertices = triangulation.vertices;

            _creatureState = CreatureState.Idle;
            SelectRandomPatrolPoint();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            CheckAndSetNextTarget();
            UpdateActionIfTargetPointReached();
        }

        public void SetPlayerHostile() => _creatureState = CreatureState.PlayerHostile;

        private void UpdateActionIfTargetPointReached()
        {
            switch (_creatureState)
            {
                case CreatureState.PlayerHostile:
                    _creatureAgent.stoppingDistance = stopFromPlayer;
                    if (!_attacking)
                        _coroutine = StartCoroutine(AttackPlayer());
                    break;

                case CreatureState.PlayerFound:
                    _creatureAgent.stoppingDistance = stopFromPlayer;
                    break;

                case CreatureState.Idle:
                    _creatureAgent.stoppingDistance = stopFromPoint;
                    break;
            }

            if (!_creatureAgent.pathPending && !_lazingAround && _creatureState == CreatureState.Idle)
            {
                if (_creatureAgent.remainingDistance <= _creatureAgent.stoppingDistance)
                {
                    if (!_creatureAgent.hasPath || _creatureAgent.velocity.sqrMagnitude == 0f)
                        _coroutine = StartCoroutine(LazeAroundPoint());
                }
            }
        }

        private void CheckAndSetNextTarget()
        {
            if (_creatureState == CreatureState.PlayerHostile)
                SetPlayerTarget();
            // TODO: Change to FOV later if there is time
            else if (Vector3.Distance(transform.position, _player.position) <= maximumDetectionDistance)
            {
                _creatureState = CreatureState.PlayerFound;
                SetPlayerTarget();
            }
            else if (_creatureState == CreatureState.PlayerFound)
            {
                _creatureState = CreatureState.Idle;
                SelectRandomPatrolPoint();
            }
        }

        private void SelectRandomPatrolPoint()
        {
            SelectRandomPointOnNavMesh();
            SetDestination(_targetVertex);
            LookTowardsTarget(_targetVertex);
        }

        private void SetPlayerTarget()
        {
            ResetAnimationOnPlayer();
            SetDestination(_player.position);
            LookTowardsTarget(_player.position);
        }

        private void SetDestination(Vector3 targetPosition)
        {
            if (!_creatureAgent.isOnNavMesh)
                return;

            _creatureAgent.SetDestination(targetPosition);
            _creatureAgent.stoppingDistance = stopFromPlayer;
        }

        private void LookTowardsTarget(Vector3 targetPosition)
        {
            Vector3 lookDirection = targetPosition - transform.position;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                    lookRotationSpeed * Time.deltaTime);
            }
        }

        private void SelectRandomPointOnNavMesh()
        {
            int randomIndex = Random.Range(0, 1000) % _vertices.Length;
            Vector3 vertex = _vertices[randomIndex];
            Vector3 targetVertex = vertex;
            _targetVertex = targetVertex;
        }

        private void ResetAnimationOnPlayer()
        {
            if (!_lazingAround)
                return;

            StopCoroutine(_coroutine);
            _lazingAround = false;
        }

        private IEnumerator AttackPlayer()
        {
            _attacking = true;

            yield return new WaitForSeconds(5);
            _attacking = false;
        }

        private IEnumerator LazeAroundPoint()
        {
            _lazingAround = true;
            yield return new WaitForSeconds(lazingTime);

            _lazingAround = false;
            SelectRandomPatrolPoint();
        }
    }
}