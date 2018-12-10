using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AstroWorld.Enemies.Drone
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DroneController : MonoBehaviour
    {
        [Header("Velocities")]
        public float lookingSpeed;

        [Header("Eyes")]
        public float lookingPoint;

        private NavMeshAgent _droneAgent;
        private DroneAttack _droneAttack;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _droneAgent = GetComponent<NavMeshAgent>();
            _droneAttack = GetComponent<DroneAttack>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            
        }
    }
}