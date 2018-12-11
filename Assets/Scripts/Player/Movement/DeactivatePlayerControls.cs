﻿using System.Collections;
using System.Collections.Generic;
using AstroWorld.Player.Data;
using AstroWorld.Player.Spawners;
using UnityEngine;

namespace AstroWorld.Player.Movement
{
    [RequireComponent(typeof(PlayerLookAtController))]
    [RequireComponent(typeof(MoveCameraWithMouse))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Animator))]
    public class DeactivatePlayerControls : MonoBehaviour
    {
        public GameObject playerCamera;
        public CannonSpawnerController cannonSpawner;

        private PlayerLookAtController _playerLookAtController;
        private MoveCameraWithMouse _moveCameraWithMouse;
        private PlayerMovement _playerMovement;

        private Animator _playerAnimator;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _playerLookAtController = GetComponent<PlayerLookAtController>();
            _moveCameraWithMouse = GetComponent<MoveCameraWithMouse>();
            _playerMovement = GetComponent<PlayerMovement>();

            _playerAnimator = GetComponent<Animator>();
        }

        public void DisablePlayerControls(bool disableCamera = true)
        {
            _playerLookAtController.enabled = false;
            _moveCameraWithMouse.enabled = false;
            _playerMovement.enabled = false;
            cannonSpawner.enabled = false;

            _playerAnimator.SetFloat(PlayerAnimationParams.HMovement, 0);
            _playerAnimator.SetFloat(PlayerAnimationParams.VMovement, 0);

            if (disableCamera)
                playerCamera.SetActive(false);
        }

        public void EnablePlayerControls(bool enableCamera = true)
        {
            _playerLookAtController.enabled = true;
            _moveCameraWithMouse.enabled = true;
            _playerMovement.enabled = true;
            cannonSpawner.enabled = true;

            if (enableCamera)
                playerCamera.SetActive(true);
        }
    }
}