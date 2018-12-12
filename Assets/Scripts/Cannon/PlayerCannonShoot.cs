using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using AstroWorld.Common;

namespace AstroWorld.Cannon
{
    public class PlayerCannonShoot : MonoBehaviour
    {
        [Header("Launch Objects")]
        public GameObject laserBurst;
        public GameObject shotEffect;
        public GameObject shootEffect;
        public Transform[] launchPoints;

        [Header("Stats")]
        public float launchSpeed;
        public CameraShakeData cameraShake;

        private const string CannonCameraName = "Cannon Camera";

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                ShootTarget();
        }

        private void ShootTarget()
        {
            for (int i = 0; i < launchPoints.Length; i++)
            {
                GameObject shotEffectInstance = Instantiate(shootEffect,
                    launchPoints[i].position, Quaternion.identity);
                shotEffectInstance.transform.SetParent(launchPoints[i]);
                shotEffectInstance.transform.localScale = Vector3.one;

                GameObject projectileInstance = Instantiate(laserBurst,
                    launchPoints[i].position, launchPoints[i].rotation);
                projectileInstance.GetComponent<Rigidbody>().velocity =
                    launchPoints[i].forward * launchSpeed;
            }

            CameraShaker.GetInstance(CannonCameraName).ShakeOnce(
                cameraShake.magnitude,
                cameraShake.roughness,
                cameraShake.fadeInTime,
                cameraShake.fadeOutTime
            );
            Instantiate(shotEffect, transform.position, Quaternion.identity);
        }
    }
}