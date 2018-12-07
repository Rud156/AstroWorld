using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Cannon
{
    public class TargetObject : MonoBehaviour
    {
        [Header("Rotation Speed")]
        public float horizontalSpeed;
        public float verticalSpeed;

        [Header("Joints")]
        public Transform weaponBase;
        public Transform weapons;

        [Header("Angles")]
        [Range(0, 20)]
        public float minAngle;
        [Range(310, 360)]
        public float maxAngle;

        private Transform _target;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (_target == null)
                return;

            TrackTargetHorizontally();
            TrackTargetVertically();
        }

        public void SetTarget(Transform target) => _target = target;

        private void TrackTargetHorizontally()
        {
            Vector3 horizontalDirection = _target.position - weaponBase.position;
            horizontalDirection.y = 0;

            if (horizontalDirection != Vector3.zero)
            {
                Quaternion horizontalLook = Quaternion.LookRotation(horizontalDirection);
                weaponBase.rotation = Quaternion.Slerp(weaponBase.rotation, horizontalLook,
                    horizontalSpeed * Time.deltaTime);
            }
        }

        private void TrackTargetVertically()
        {
            Vector3 verticalDirection = _target.position - weapons.position;

            if (verticalDirection != Vector3.zero)
            {
                Quaternion verticalLook = Quaternion.LookRotation(verticalDirection);
                Vector3 rotation = Quaternion.Slerp(weapons.localRotation, verticalLook,
                    verticalSpeed * Time.deltaTime).eulerAngles;

                rotation.y = 0;
                if (rotation.x >= 0 && rotation.x <= minAngle ||
                    rotation.x >= maxAngle && rotation.x <= 360)
                    weapons.localRotation = Quaternion.Euler(rotation);
            }
        }
    }
}