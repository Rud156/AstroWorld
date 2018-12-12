using AstroWorld.Cannon;
using AstroWorld.Extras;
using AstroWorld.Player.StatusDisplay;
using AstroWorld.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Player.Spawners
{
    public class CannonSpawnerController : MonoBehaviour
    {
        [Header("Main Object")]
        public GameObject laserCannon;
        public Transform referencePoint;

        [Header("Stats")]
        [Range(3, 10)]
        public float spawnBeforeDistance;
        public float collectionTime;
        public float cannonSlopeAllowed = 10;

        private PlayerDeathController _playerDeathController;

        private bool _cannonInRange;
        private float _currentCollectionTime;
        private bool _cannonSpawned;

        private Image _displayImage;
        private GameObject _cannonInstance;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _currentCollectionTime = collectionTime;
            _playerDeathController = GetComponentInParent<PlayerDeathController>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            CheckDeployAndCannon();
            CheckAndPickUpCannon();

            CheckAndUseCannon();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagManager.Cannon))
                _cannonInRange = true;
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagManager.Cannon))
            {
                _cannonInRange = false;
                ResetTimerImage();
            }
        }

        private void CheckAndUseCannon()
        {
            if (Input.GetKeyDown(Controls.InteractionKey) && _cannonInRange)
                _cannonInstance.GetComponent<SwitchPlayerCannonCamera>().ActivateCannonControl();
        }

        private void CheckDeployAndCannon()
        {
            if (_cannonSpawned)
                return;

            if (Input.GetKeyDown(Controls.CannonSpawnKey))
            {
                Vector3 spawnPoint = GetSpawningPoint();
                if (spawnPoint == Vector3.zero)
                    return;

                if (_cannonInstance == null)
                {
                    _cannonInstance = Instantiate(
                        laserCannon,
                        spawnPoint,
                        laserCannon.transform.rotation
                    );
                    _displayImage = _cannonInstance.GetComponentInChildren<Image>();
                    _playerDeathController.SetCannonController(
                        _cannonInstance.GetComponent<SwitchPlayerCannonCamera>()
                    );
                }
                else
                {
                    _cannonInstance.SetActive(true);
                    _cannonInstance.transform.position = spawnPoint;
                }

                _cannonSpawned = true;
                ResetTimerImage();
            }
        }

        private void CheckAndPickUpCannon()
        {
            if (!_cannonSpawned)
                return;

            if (Input.GetKey(Controls.CannonSpawnKey) && _cannonInRange)
            {
                _currentCollectionTime -= Time.deltaTime;
                _displayImage.enabled = true;
                _displayImage.fillAmount =
                    ExtensionFunctions.Map(_currentCollectionTime, 0, collectionTime, 0, 1);

                if (_currentCollectionTime <= 0)
                {
                    _cannonInstance.SetActive(false);
                    _cannonSpawned = false;
                    ResetTimerImage();
                }
            }
            else if (!Input.GetKey(Controls.CannonSpawnKey))
                ResetTimerImage();
        }

        private Vector3 GetSpawningPoint()
        {
            Vector3 destination = referencePoint.position +
                referencePoint.forward * spawnBeforeDistance;

            RaycastHit hit;
            if (Physics.Linecast(referencePoint.position, destination, out hit))
                destination = referencePoint.position + referencePoint.forward * (hit.distance - 1);

            if (Physics.Raycast(destination, Vector3.down, out hit))
            {
                destination = hit.point;

                float angle = Vector3.Angle(hit.normal, transform.forward);
                float normalizedAngle = ExtensionFunctions.To360Angle(angle);

                if (normalizedAngle - 90 > cannonSlopeAllowed)
                    return Vector3.zero;
                else
                    return destination;
            }

            return Vector3.zero;
        }

        private void ResetTimerImage()
        {
            _currentCollectionTime = collectionTime;
            _displayImage.enabled = false;
        }
    }
}