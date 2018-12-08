using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AstroWorld.Enemies.Creature
{
    [RequireComponent(typeof(CreatureAttack))]
    [RequireComponent(typeof(CreatureLaze))]
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

        [Header("Eyes")]
        public Transform lookingPoint;
        [Range(0, 360)]
        public int lookAngle;
        [Range(0, 20)]
        public int angleToleranceLevel;

        private NavMeshAgent _creatureAgent;
        private Vector3[] _vertices;

        private Transform _currentTarget;
        private Transform _player;

        private CreatureAttack _creatureAttack;
        private CreatureLaze _creatureLaze;

        private CreatureState _creatureState;
        private float _currentNormalizedAngle;
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
            _creatureAttack = GetComponent<CreatureAttack>();
            _creatureLaze = GetComponent<CreatureLaze>();

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

        private void CheckAndSetNextTarget()
        {
            if (_creatureState == CreatureState.PlayerHostile)
                SetPlayerTarget();
            else
            {
                float angleWRTTarget = CreatureHelpers
                    .CheckTargetInsideFOV(_player, maximumDetectionDistance, lookAngle, lookingPoint);

                if (angleWRTTarget != -1)
                {
                    _creatureState = CreatureState.PlayerFound;
                    _currentNormalizedAngle = angleWRTTarget;
                    SetPlayerTarget();
                }
                else if (_creatureState == CreatureState.PlayerFound)
                {
                    _creatureState = CreatureState.Idle;
                    SelectRandomPatrolPoint();
                }
            }
        }


        private void UpdateActionIfTargetPointReached()
        {
            switch (_creatureState)
            {
                case CreatureState.PlayerHostile:
                    _creatureAgent.stoppingDistance = stopFromPlayer;
                    if (!_attacking &&
                        CreatureHelpers.IsAngleWithinToleranceLevel(_currentNormalizedAngle,
                        angleToleranceLevel))
                    {
                        _coroutine = StartCoroutine(AttackPlayer());
                    }
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

            float attackingTime = _creatureAttack.AttackTarget(_player, true);
            yield return new WaitForSeconds(attackingTime);

            _attacking = false;
        }

        private IEnumerator LazeAroundPoint()
        {
            _lazingAround = true;

            float lazingTime = _creatureLaze.LazeAroundSpot();
            yield return new WaitForSeconds(lazingTime);

            _lazingAround = false;
            SelectRandomPatrolPoint();
        }
    }
}