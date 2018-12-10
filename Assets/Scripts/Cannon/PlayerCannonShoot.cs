using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Cannon
{
    public class PlayerCannonShoot : MonoBehaviour
    {
        [Header("Launch Objects")]
        public GameObject laserBurst;
        public GameObject shootEffect;
        public Transform[] launchPoints;

        [Header("Movement Stats")]
        public float launchSpeed;

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
        }
    }
}