using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Enemies.Base
{
    public abstract class Attack : MonoBehaviour
    {

        [Header("Effects")]
        public GameObject enemyBullet;
        public GameObject launchEffect;

        [Header("Launch Points")]
        public Transform[] launchPoints;

        [Header("Launch Stats")]
        public float attackTime;
        public float playerBaseOffset;
        public float launchSpeed;

        public abstract float AttackTarget(Transform target, bool usePlayerOffset = false);

        public virtual void EndAttack() { }
    }
}