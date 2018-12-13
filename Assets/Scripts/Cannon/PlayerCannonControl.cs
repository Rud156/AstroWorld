using System.Collections;
using System.Collections.Generic;
using AstroWorld.Extras;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Cannon
{
    public class PlayerCannonControl : MonoBehaviour
    {

        [Header("Moving Parts")]
        public Transform platformHolder;
        public Transform weapons;

        [Header("Velocities")]
        public float horizontalSpeed;
        public float verticalSpeed;

        [Header("Limiting Angles")]
        [Range(0, 360)]
        public int minCameraAngle = 30;
        [Range(0, 360)]
        public int maxCameraAngle = 340;

        private float _yaw;
        private float _pitch;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _yaw = platformHolder.rotation.eulerAngles.y;
            _pitch = weapons.rotation.eulerAngles.x;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            RotateVertically();
            RotateHorizontally();
        }

        private void RotateHorizontally()
        {
            float mouseX = Input.GetAxis(Controls.MouseX);
            _yaw += mouseX * horizontalSpeed * Time.deltaTime;
            platformHolder.eulerAngles = Vector3.up * _yaw;
        }

        private void RotateVertically()
        {

            float mouseY = Input.GetAxis(Controls.MouseY);
            _pitch += -mouseY * verticalSpeed * Time.deltaTime;

            if (_pitch < 0 || _pitch > 360)
                _pitch = ExtensionFunctions.To360Angle(_pitch);

            if (_pitch > minCameraAngle && _pitch < maxCameraAngle)
            {
                float diffToMinAngle = Mathf.Abs(minCameraAngle - _pitch);
                float diffToMaxAngle = Mathf.Abs(maxCameraAngle - _pitch);

                if (diffToMinAngle < diffToMaxAngle)
                    _pitch = minCameraAngle;
                else
                    _pitch = maxCameraAngle;
            }

            weapons.localRotation = Quaternion.Euler(_pitch, 0, 0);
        }
    }
}