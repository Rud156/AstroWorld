using System.Collections;
using System.Collections.Generic;
using AstroWorld.Player.Movement;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Cannon
{
    public class SwitchPlayerCannonCamera : MonoBehaviour
    {
        [Header("Cannon Parts")]
        public Transform playerLockPoint;
        public GameObject cannonCamera;

        private Transform _player;
        private DeactivatePlayerControls _deactivatePlayer;

        private PlayerCannonShoot _playerCannonShoot;
        private PlayerCannonControl _playerCannonControl;

        private bool _cannonControlActive;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _playerCannonControl = GetComponent<PlayerCannonControl>();
            _playerCannonShoot = GetComponent<PlayerCannonShoot>();

            _player = GameObject.FindGameObjectWithTag(TagManager.Player)?.transform;
            _deactivatePlayer = _player.GetComponent<DeactivatePlayerControls>();

            DeactivateCannonControl();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update() => CheckAndLeaveCannon();

        private void CheckAndLeaveCannon()
        {
            if (Input.GetKeyDown(Controls.CannonActivateKey) && _cannonControlActive)
                DeactivateCannonControl();
        }

        public void ActivateCannonControl()
        {
            cannonCamera.SetActive(true);
            _deactivatePlayer.DisablePlayerControls();

            _player.position = playerLockPoint.position;
            _player.SetParent(transform);

            _playerCannonControl.enabled = true;
            _playerCannonShoot.enabled = true;
            StartCoroutine(DelayCannonActivation());
        }

        public void DeactivateCannonControl()
        {
            cannonCamera.SetActive(false);
            _player.SetParent(null);

            _deactivatePlayer.EnablePlayerControls();

            _playerCannonControl.enabled = false;
            _playerCannonShoot.enabled = false;
            _cannonControlActive = false;
        }

        IEnumerator DelayCannonActivation()
        {
            yield return new WaitForSeconds(0.3f);
            _cannonControlActive = true;
        }
    }
}