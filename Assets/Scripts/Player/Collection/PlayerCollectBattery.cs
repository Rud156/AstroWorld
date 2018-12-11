using System.Collections;
using System.Collections.Generic;
using AstroWorld.Player.StatusDisplay;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Collection
{
    public class PlayerCollectBattery : MonoBehaviour
    {
        public GameObject collectionEffect;

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(TagManager.Battery))
            {
                Instantiate(collectionEffect, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);

                PlayerBatteryCountManager.instance.CollectBattery();
            }
        }
    }
}