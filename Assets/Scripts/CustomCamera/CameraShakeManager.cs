using System.Collections;
using System.Collections.Generic;
using AstroWorld.Common;
using EZCameraShake;
using UnityEngine;

namespace AstroWorld.CustomCamera
{
    public class CameraShakeManager : MonoBehaviour
    {
        #region Singleton

        public static CameraShakeManager instance;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (instance == null)
                instance = this;

            if (instance != this)
                Destroy(gameObject);
        }

        #endregion Singleton

        public CameraShakeData shootShake;
        public CameraShakeData explosionShake;

        public Vector3 shootInfluence;
        public Vector3 explosionInfluence;

        public void ShakeShoot()
        {
            CameraShaker.Instance.DefaultPosInfluence = shootInfluence;
            CameraShaker.Instance.ShakeOnce(
                shootShake.magnitude,
                shootShake.roughness,
                shootShake.fadeInTime,
                shootShake.fadeOutTime
            );
        }

        public void ShakeExplosion(float magnitudeAmount)
        {
            CameraShaker.Instance.DefaultPosInfluence = explosionInfluence;
            CameraShaker.Instance.ShakeOnce(
                explosionShake.magnitude * magnitudeAmount,
                explosionShake.roughness,
                explosionShake.fadeInTime,
                explosionShake.fadeOutTime
            );
        }
    }
}