using System.Collections;
using System.Collections.Generic;
using AstroWorld.Common;
using AstroWorld.Player.Data;
using AstroWorld.Player.Movement;
using UnityEngine;

namespace AstroWorld.Player.StatusDisplay
{
    [RequireComponent(typeof(HealthSetter))]
    [RequireComponent(typeof(DeactivatePlayerControls))]
    public class PlayerDeathManager : MonoBehaviour
    {
        private HealthSetter _healthSetter;
        private DeactivatePlayerControls _deactivatePlayer;
        private Animator _playerAnimator;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _deactivatePlayer = GetComponent<DeactivatePlayerControls>();

            _healthSetter = GetComponent<HealthSetter>();
            _healthSetter.healthZero += AnimatePlayerDead;
        }

        private void AnimatePlayerDead()
        {
            _deactivatePlayer.DisablePlayerControls(false);
            _playerAnimator.SetBool(PlayerAnimationParams.Dead, true);
        }
    }
}