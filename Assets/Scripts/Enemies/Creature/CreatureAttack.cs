﻿using System.Collections;
using System.Collections.Generic;
using AstroWorld.Enemies.Base;
using UnityEngine;

namespace AstroWorld.Enemies.Creature
{
    public class CreatureAttack : Attack
    {

        [Header("Attack Animations")]
        public float timeDiffSameAttack;
        public float totalAttackTimes;
        public Animator droidAnimator;

        private Transform _target;
        private bool _usePlayerOffset;

        private const string AttackAnimationParam = "Attacking";

        public override float AttackTarget(Transform target, bool usePlayerOffset = false)
        {
            _target = target;
            _usePlayerOffset = usePlayerOffset;
            droidAnimator.SetBool(AttackAnimationParam, true);

            return base.attackTime;
        }

        public override void EndAttack() => droidAnimator.SetBool(AttackAnimationParam, false);

        // Called From Animator
        public void AttackPlayer()
        {
            for (int i = 0; i < totalAttackTimes; i++)
                StartCoroutine(AttackDelayedStart(i * timeDiffSameAttack));
        }

        private IEnumerator AttackDelayedStart(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            for (int i = 0; i < base.launchPoints.Length; i++)
            {
                Vector3 position = _usePlayerOffset ?
                    _target.position + Vector3.up * base.playerBaseOffset :
                    _target.position;

                Quaternion lookRotation = Quaternion.LookRotation(position -
                    base.launchPoints[i].position);
                base.launchPoints[i].transform.rotation = lookRotation;

                Instantiate(base.launchEffect, base.launchPoints[i].position, lookRotation);
                Instantiate(base.shotEffect, transform.position, Quaternion.identity);

                GameObject bulletInstance = Instantiate(base.enemyBullet,
                    base.launchPoints[i].transform.position, Quaternion.identity);
                bulletInstance.transform.rotation = lookRotation;
                bulletInstance.GetComponent<Rigidbody>().velocity =
                    base.launchPoints[i].transform.forward * base.launchSpeed;
            }
        }
    }
}