using System.Collections;
using System.Collections.Generic;
using AstroWorld.Player.StatusDisplay;
using AstroWorld.UI;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Collection
{
    public class PlayerEnterShip : MonoBehaviour
    {
        private bool _isPlayerNearby;
        private bool _triggerActivated;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start() => _isPlayerNearby = false;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagManager.Ship))
                _isPlayerNearby = true;
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagManager.Ship))
                _isPlayerNearby = false;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (_isPlayerNearby && Input.GetKeyDown(Controls.InteractionKey) &&
                PlayerBatteryCountManager.instance.IsShipAccessible() && !_triggerActivated)
            {
                Fader.instance.StartFadeOut();
                _triggerActivated = true;
            }
        }
    }
}