using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Movement
{
    public class PlayerLookAtController : MonoBehaviour
    {
        [Header("Rotation")]
        public float rotationSpeed;

        private float _yaw;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start() => _yaw = transform.rotation.eulerAngles.y;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update() => RotatePlayerOnMouse();

        private void RotatePlayerOnMouse()
        {
            float mouseX = Input.GetAxis(Controls.MouseX);
            _yaw += mouseX * rotationSpeed * Time.deltaTime;
            transform.eulerAngles = Vector3.up * _yaw;
        }
    }
}