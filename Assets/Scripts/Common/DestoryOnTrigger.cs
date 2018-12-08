using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Common
{
    public class DestoryOnTrigger : MonoBehaviour
    {
        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other) => Destroy(gameObject);
    }
}