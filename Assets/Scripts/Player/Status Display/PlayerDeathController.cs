using System.Collections;
using System.Collections.Generic;
using AstroWorld.Cannon;
using AstroWorld.Common;
using AstroWorld.Player.Data;
using AstroWorld.Player.Movement;
using UnityEngine;

namespace AstroWorld.Player.StatusDisplay
{
    [RequireComponent(typeof(HealthSetter))]
    [RequireComponent(typeof(DeactivatePlayerControls))]
    public class PlayerDeathController : MonoBehaviour
    {
        private HealthSetter _healthSetter;
        private DeactivatePlayerControls _deactivatePlayer;
        private Animator _playerAnimator;
        private SwitchPlayerCannonCamera _switchPlayerCannon;
        private AudioSource _deathAudio;

        private bool _deathAnimationPlayed;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _deactivatePlayer = GetComponent<DeactivatePlayerControls>();
            _deathAudio = GetComponent<AudioSource>();

            _healthSetter = GetComponent<HealthSetter>();
            _healthSetter.healthZero += AnimatePlayerDead;
        }

        public void SetCannonController(SwitchPlayerCannonCamera switchPlayerCannon) =>
            _switchPlayerCannon = switchPlayerCannon;

        private void AnimatePlayerDead()
        {
            if (_deathAnimationPlayed)
                return;

            _deathAnimationPlayed = true;

            if (_switchPlayerCannon == null)
            {
                _switchPlayerCannon.DeactivateCannonControl();
                _switchPlayerCannon.MakeCannonUnusable();
            }

            _deathAudio.Play();
            _deactivatePlayer.DisablePlayerControls(false);
            _playerAnimator.SetBool(PlayerAnimationParams.Dead, true);
        }
    }
}