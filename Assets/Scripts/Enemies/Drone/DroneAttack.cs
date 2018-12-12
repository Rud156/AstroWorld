using System.Collections;
using System.Collections.Generic;
using AstroWorld.Enemies.Base;
using UnityEngine;

public class DroneAttack : Attack
{
    public override float AttackTarget(Transform target, bool usePlayerOffset = false)
    {
        for (int i = 0; i < base.launchPoints.Length; i++)
        {
            Vector3 position = usePlayerOffset ? target.position +
                Vector3.up * base.playerBaseOffset : target.position;

            Quaternion lookRotation = Quaternion.LookRotation(position -
                base.launchPoints[i].position);
            base.launchPoints[i].transform.rotation = lookRotation;

            Instantiate(base.launchEffect, base.launchPoints[i].position, lookRotation);
            Instantiate(base.shotEffect, transform.position, Quaternion.identity);

            GameObject bulletInstance = Instantiate(base.enemyBullet,
                base.launchPoints[i].position, Quaternion.identity);
            bulletInstance.transform.rotation = lookRotation;
            bulletInstance.GetComponent<Rigidbody>().velocity = base.launchPoints[i].forward *
                base.launchSpeed;
        }

        return base.attackTime;
    }
}
