using System.Collections;
using System.Collections.Generic;
using AstroWorld.Extras;
using UnityEngine;

namespace AstroWorld.Cannon
{
    public class TargetShoot : MonoBehaviour
    {
        [Header("Launch Objects")]
        public GameObject laserBurst;
        public GameObject shootEffect;
        public Transform platformBase;
        public Transform[] launchPoints;

        [Header("Controls")]
        [Range(0, 30)]
        public int angleToleranceLevel;
        public float launchSpeed;

        [Header("Time")]
        public float fireRate;

        [Header("Debug")]
        public bool shootOnStart;

        private Transform _target;
        private Coroutine _coroutine;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if (shootOnStart)
                StartShooting();
        }

        public void SetTarget(Transform target) => _target = target;

        public void StartShooting() => _coroutine = StartCoroutine(ShootTarget());

        public void StopShooting() => StopCoroutine(_coroutine);

        private bool IsTargetInAngleToleranceLevel()
        {
            if (_target == null)
                return false;

            Vector3 modifiedPlayerPosition = new Vector3(_target.position.x, 0, _target.position.z);
            Vector3 modifiedLookingPosition =
                new Vector3(platformBase.position.x, 0, platformBase.position.z);

            Vector3 lookDirection = modifiedPlayerPosition - modifiedLookingPosition;
            float angleToPlayer = Vector3.Angle(lookDirection, platformBase.forward);
            float normalizedAngle = ExtensionFunctions.To360Angle(angleToPlayer);

            if (normalizedAngle <= angleToleranceLevel)
                return true;
            else
                return false;
        }

        private IEnumerator ShootTarget()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);

                if (_target != null && IsTargetInAngleToleranceLevel())
                {
                    for (int i = 0; i < launchPoints.Length; i++)
                    {
                        Vector3 direction = _target.transform.position -
                                launchPoints[i].position;
                        Quaternion lookRotation = Quaternion.LookRotation(direction);

                        GameObject shotEffectInstance = Instantiate(shootEffect,
                            launchPoints[i].position, Quaternion.identity);
                        shotEffectInstance.transform.SetParent(launchPoints[i]);
                        shotEffectInstance.transform.localScale = Vector3.one;

                        GameObject projectileInstance = Instantiate(laserBurst,
                            launchPoints[i].position, Quaternion.identity);
                        projectileInstance.transform.rotation = lookRotation;
                        projectileInstance.GetComponent<Rigidbody>().velocity =
                            launchPoints[i].forward * launchSpeed;
                    }
                }
            }
        }
    }
}