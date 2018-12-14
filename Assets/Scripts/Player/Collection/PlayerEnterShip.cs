using System.Collections;
using System.Collections.Generic;
using AstroWorld.Common;
using AstroWorld.Player.StatusDisplay;
using AstroWorld.Scenes.Loading;
using AstroWorld.Scenes.Main;
using AstroWorld.UI;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Collection
{
    [RequireComponent(typeof(Collider))]
    public class PlayerEnterShip : MonoBehaviour
    {
        private bool _isShipNearby;
        private bool _triggerActivated;

        private HealthSetter _healthSetter;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _isShipNearby = false;
            _healthSetter = GetComponent<HealthSetter>();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagManager.Ship))
                _isShipNearby = true;
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagManager.Ship))
                _isShipNearby = false;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (_isShipNearby && Input.GetKeyDown(Controls.InteractionKey) &&
                PlayerBatteryCountManager.instance.CollectedRequiredBattries() && !_triggerActivated)
            {
                _healthSetter.enabled = false;

                Fader.instance.StartFadeOut();
                InfoTextManager.instance.DisplayText("You completed the mission !!!", Color.green);

                _triggerActivated = true;
                NextSceneData.missionComplete = true;
            }
        }
    }
}