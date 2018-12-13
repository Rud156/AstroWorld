using System.Collections;
using System.Collections.Generic;
using AstroWorld.Extras;
using AstroWorld.Spawners;
using AstroWorld.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Scenes.Main
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager instance;

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

        [Header("Chaos Activator")]
        public int maxChaosLevel;
        public float maxDronesToSpawn;

        [Header("Chaos Display")]
        public AudioSource backgroundMusic;
        public Image chaosImage;
        public Text chaosText;

        [Header("Audio Control")]
        [Range(0, 1)]
        public float minAudioPitch;
        [Range(0, 1)]
        public float maxAudioPitch;

        private int _chaosLevel;
        private bool _chaosCreated;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Fader.instance.StartFadeIn();
            UpdateUIAndMusic();
        }

        public void IncrementChaosLevel()
        {
            _chaosLevel += 1;
            UpdateUIAndMusic();

            if (_chaosLevel >= maxChaosLevel && !_chaosCreated)
            {
                _chaosCreated = true;
                StartCoroutine(SpawnDefenceDrones());
            }
        }

        private void UpdateUIAndMusic()
        {
            float chaosRatio = _chaosLevel / maxChaosLevel;
            chaosImage.fillAmount = chaosRatio;
            backgroundMusic.pitch = ExtensionFunctions.Map(
                chaosRatio,
                0, 1,
                minAudioPitch, maxAudioPitch
            );

            chaosText.text = $"{_chaosLevel} / {maxChaosLevel}\nChaos Level";
        }

        private IEnumerator SpawnDefenceDrones()
        {
            DroneSpawner.instance.StartSpawn();
            yield return new WaitForSeconds(DroneSpawner.instance.spawnRate * maxDronesToSpawn);
            DroneSpawner.instance.StopSpawn();
        }
    }
}