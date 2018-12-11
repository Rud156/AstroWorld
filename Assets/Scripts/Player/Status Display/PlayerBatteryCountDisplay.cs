using System.Collections;
using System.Collections.Generic;
using AstroWorld.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Player.StatusDisplay
{
    public class PlayerBatteryCountDisplay : MonoBehaviour
    {
        #region Singleton

        public static PlayerBatteryCountDisplay instance;

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

        public Image batteryImage;

        private int _maxBatteryRequired;
        private int _currentBatteryCount;
        private bool _shipAccessible;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _maxBatteryRequired = Random.Range(10, CreatureSpawner.instance.initialSpawnCount);
            _currentBatteryCount = 0;
        }

        public void CollectBattery()
        {
            _currentBatteryCount += 1;
            if (_currentBatteryCount >= _maxBatteryRequired)
                _shipAccessible = true;

            float ratio = Mathf.Clamp01(_currentBatteryCount / _maxBatteryRequired);
            batteryImage.fillAmount = ratio;
        }

        public bool IsShipAccessible() => _shipAccessible;
    }
}