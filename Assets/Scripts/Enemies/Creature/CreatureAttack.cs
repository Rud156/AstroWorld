using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Enemies.Creature
{
    public class CreatureAttack : MonoBehaviour
    {
        [Header("Effects")]
        public GameObject droidBullet;
        public GameObject launchEffect;

        [Header("Launch Points")]
        public Transform[] launchPoints;

        [Header("Launch Stats")]
        public float attackTime;
        public float playerBaseOffset;
        public float launchSpeed;

        [Header("Attack Animations")]
        public float timeDiffSameAttack;
        public float totalAttackTimes;
        public Animator droidAnimator;

        private Transform _target;
        private bool _usePlayerOffset;

        private const string AttackAnimationParam = "Attacking";

        public float Attack(Transform target, bool usePlayerOffset = false)
        {
            _target = target;
            _usePlayerOffset = usePlayerOffset;
            droidAnimator.SetBool(AttackAnimationParam, true);

            return attackTime;
        }

        public void EndAttack() => droidAnimator.SetBool(AttackAnimationParam, false);

        public void AttackTarget()
        {
            for (int i = 0; i < totalAttackTimes; i++)
                StartCoroutine(AttackDelayedStart(i * timeDiffSameAttack));
        }

        private IEnumerator AttackDelayedStart(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            for (int i = 0; i < launchPoints.Length; i++)
            {
                Vector3 position = _usePlayerOffset ? _target.position +
                    Vector3.up * playerBaseOffset :
                    _target.position;

                Quaternion lookRotation = Quaternion
                    .LookRotation(position - launchPoints[i].position);
                launchPoints[i].transform.rotation = lookRotation;

                Instantiate(launchEffect, launchPoints[i].position, lookRotation);

                GameObject bulletInstance = Instantiate(droidBullet,
                    launchPoints[i].transform.position, Quaternion.identity);
                bulletInstance.transform.rotation = lookRotation;
                bulletInstance.GetComponent<Rigidbody>().velocity =
                    launchPoints[i].transform.forward * launchSpeed;
            }
        }
    }
}