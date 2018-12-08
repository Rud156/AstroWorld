using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Cannon
{
    [RequireComponent(typeof(TargetObject))]
    [RequireComponent(typeof(TargetShoot))]
    public class FindTarget : MonoBehaviour
    {
        public Transform platformBase;
        public float maxDistanceToTarget;

        private Transform _enemyPoints;
        private Transform _currentTarget;

        private TargetObject _targetObject;
        private TargetShoot _targetShoot;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _enemyPoints = GameObject.FindGameObjectWithTag(TagManager.EnemyHolder)?.transform;
            _targetObject = GetComponent<TargetObject>();
            _targetShoot = GetComponent<TargetShoot>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            int childCount = _enemyPoints.childCount;
            float minDistance = float.MaxValue;
            _currentTarget = null;

            for (int i = 0; i < childCount; i++)
            {
                float distance = Vector3.Distance(platformBase.position,
                    _enemyPoints.GetChild(i).position);
                if (distance <= maxDistanceToTarget && distance <= minDistance)
                {
                    _currentTarget = _enemyPoints.GetChild(i);
                    minDistance = distance;
                }
            }

            if (_currentTarget != null)
            {
                _targetObject.SetTarget(_currentTarget);
                _targetShoot.SetTarget(_currentTarget);
            }
        }
    }
}