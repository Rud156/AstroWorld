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
        public Transform lookPoint;
        public Transform[] launchPoints;

        [Header("Controls")]
        [Range(0, 30)]
        public int angleToleranceLevel;
        public float launchSpeed;

        [Header("Time")]
        public float fireRate;

        private Transform _target;

        public void SetTarget(Transform target) => _target = target;

        private bool IsTargetInAngleToleranceLevel()
        {
            if (_target == null)
                return false;

            Vector3 modifiedPlayerPosition = new Vector3(_target.position.x, 0, _target.position.z);
            Vector3 modifiedLookingPosition =
                new Vector3(lookPoint.position.x, 0, lookPoint.position.z);

            Vector3 lookDirection = modifiedPlayerPosition - modifiedLookingPosition;
            float angleToPlayer = Vector3.Angle(lookDirection, lookPoint.forward);
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
                        shotEffectInstance.transform.rotation = lookRotation;

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