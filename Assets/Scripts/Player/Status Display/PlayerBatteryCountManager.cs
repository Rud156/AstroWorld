using System.Collections;
using System.Collections.Generic;
using AstroWorld.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Player.StatusDisplay
{
    public class PlayerBatteryCountManager : MonoBehaviour
    {
        #region Singleton

        public static PlayerBatteryCountManager instance;

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
        public Text batteryText;

        private int _maxBatteryRequired;
        private int _currentBatteryCount;
        private bool _batteriesCollected;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _maxBatteryRequired = Random.Range(5, CreatureSpawner.instance.initialSpawnCount / 2);
            _currentBatteryCount = 0;

            UpdateUIWithCount();
        }

        public void CollectBattery()
        {
            _currentBatteryCount += 1;
            if (_currentBatteryCount >= _maxBatteryRequired)
                _batteriesCollected = true;

            UpdateUIWithCount();
        }

        private void UpdateUIWithCount()
        {
            float ratio = Mathf.Clamp01(_currentBatteryCount / (float)_maxBatteryRequired);
            batteryImage.fillAmount = ratio;

            batteryText.text = $"{_currentBatteryCount} / {_maxBatteryRequired}\nBatteries";
        }

        public bool CollectedRequiredBattries() => _batteriesCollected;
    }
}