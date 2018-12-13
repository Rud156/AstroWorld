using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Movement
{
    public class PlayerLookAtController : MonoBehaviour
    {
        [Header("Rotation")]
        public float rotationSpeed;

        private float _yaw;

        private bool _rotationActive;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _yaw = transform.rotation.eulerAngles.y;
            _rotationActive = true;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update() => RotatePlayerOnMouse();

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable() => _rotationActive = true;

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            _rotationActive = false;
        }

        private void RotatePlayerOnMouse()
        {
            float mouseX = _rotationActive ? Input.GetAxis(Controls.MouseX) : 0;
            _yaw += mouseX * rotationSpeed * Time.deltaTime;
            transform.eulerAngles = Vector3.up * _yaw;
        }
    }
}