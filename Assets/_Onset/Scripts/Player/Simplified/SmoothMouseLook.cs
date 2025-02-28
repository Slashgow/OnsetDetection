// original source: http://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/
// Very simple smooth mouselook modifier for the MainCamera in Unity
// by Francis R. Griffiths-Keam - www.runningdimensions.com 
// Modified: Escape key for hide/show & lock/unlock mouse

using UnityEngine;
namespace UnityLibrary
{
    public class SmoothMouseLook : MonoBehaviour
    {
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("How far in degrees can you move the camera up"), Range(-180f, 0f)]
        public float LeftClamp = -90f;

        [Tooltip("How far in degrees can you move the camera down"), Range(0f, 180f)]
        public float RightClamp = 90f;


        Vector2 _mouseAbsolute;
        Vector2 _smoothMouse;

        public bool lockCursor;
        public Vector2 smoothing = new Vector2(3, 3);
        public Vector2 targetDirection;

        private float _targetRotation;
        private float _rotationVelocity;
        private GameObject _mainCamera;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        void Start()
        {
            // Set target direction to the camera's initial orientation.
            targetDirection = transform.rotation.eulerAngles;
        }

        private void Update()
        {
            RecenterPlayer();
        }
        public void RecenterPlayer(float damping = 0)
        {
            _targetRotation = _mainCamera.transform.eulerAngles.y;
            //Debug.Log(_targetRotation);
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            //Debug.Log(rotation);
            if (!float.IsNaN(rotation))
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        void LateUpdate()
        {
            if (GameManager.Instance.IsPaused)
                return;

            // Allow the script to clamp based on a desired target value.
            Quaternion targetOrientation = Quaternion.Euler(targetDirection);

            // Get raw mouse input for a cleaner reading on more sensitive mice.
            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(SettingManager.Instance.CurrentSensitivity * smoothing.x, SettingManager.Instance.CurrentSensitivity * smoothing.y));

            // Interpolate mouse movement over time to apply smoothing delta.
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            // Find the absolute mouse movement value from point zero.
            _mouseAbsolute += _smoothMouse;

            // Clamp and apply the local x value first, so as not to be affected by world transforms.

            _mouseAbsolute.x = ClampAngle(_mouseAbsolute.x, LeftClamp, RightClamp);

            //var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
            //CinemachineCameraTarget.transform.localRotation = xRotation;

            // Then clamp and apply the global y value.
            _mouseAbsolute.y = ClampAngle(_mouseAbsolute.y, BottomClamp, TopClamp);

            //var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            //CinemachineCameraTarget.transform.localRotation *= yRotation;
            //CinemachineCameraTarget.transform.rotation *= targetOrientation;

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(-_mouseAbsolute.y,
           _mouseAbsolute.x, 0.0f);
        }


        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}