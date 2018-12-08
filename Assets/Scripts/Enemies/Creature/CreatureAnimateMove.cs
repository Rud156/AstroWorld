using AstroWorld.Extras;
using UnityEngine;
using UnityEngine.AI;

namespace AstroWorld.Enemies.Creature
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CreatureAnimateMove : MonoBehaviour
    {
        private Animator _creatureAnimator;
        private NavMeshAgent _creatureAgent;
        private const string AnimatorMovement = "Movement";

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _creatureAgent = GetComponent<NavMeshAgent>();
            _creatureAnimator = GetComponent<Animator>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            float maxVelocity = _creatureAgent.speed * _creatureAgent.speed;
            float currentVelocity = _creatureAgent.velocity.sqrMagnitude;

            float movementSpeed = ExtensionFunctions.Map(currentVelocity, 0, maxVelocity, 0, 1);
            _creatureAnimator.SetFloat(AnimatorMovement, movementSpeed);
        }
    }
}