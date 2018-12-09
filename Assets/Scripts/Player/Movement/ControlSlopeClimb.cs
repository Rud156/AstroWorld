using AstroWorld.Extras;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class ControlSlopeClimb : MonoBehaviour
    {
        public Transform raycastPoint;
        [Range(0, 360)]
        public float maxAngleAllowed;

        private RaycastHit _hit;
        private Rigidbody _playerRB;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start() => _playerRB = GetComponent<Rigidbody>();

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        void LateUpdate() => StopPlayerOnExtremeSlope();

        private void StopPlayerOnExtremeSlope()
        {
            if (Physics.Raycast(raycastPoint.position, Vector3.down, out _hit))
            {
                float moveX = Input.GetAxis(Controls.HorizontalAxis);
                float moveZ = Input.GetAxis(Controls.VerticalAxis);

                Vector3 playerAxis = transform.forward;
                if (moveZ < 0)
                    playerAxis = -transform.forward;
                else if (moveX > 0)
                    playerAxis = transform.right;
                else if (moveX < 0)
                    playerAxis = -transform.right;

                float angle = Vector3.Angle(_hit.normal, playerAxis);
                float normalizedAngle = ExtensionFunctions.To360Angle(angle);

                if (normalizedAngle - 90 > maxAngleAllowed)
                    _playerRB.velocity = Vector3.zero;
            }
        }
    }
}