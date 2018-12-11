using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Common
{
    public class DestoryOnTrigger : MonoBehaviour
    {
        public bool displayImpact = true;
        public GameObject impact;

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (displayImpact)
                Instantiate(impact, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}