using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Scenes.Main
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager _instance;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (_instance == null)
                _instance = this;

            if (_instance != this)
                Destroy(gameObject);
        }

        #endregion Singleton

		public int maxChaosLevel;

		private int _chaosLevel;
    }
}