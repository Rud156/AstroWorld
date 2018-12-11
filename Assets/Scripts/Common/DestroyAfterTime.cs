using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Common
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float destroyTime;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start() => Destroy(gameObject, destroyTime);
    }
}