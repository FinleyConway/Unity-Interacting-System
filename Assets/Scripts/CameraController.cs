using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Location")]
    [SerializeField] private Transform _cameraHolder = null;

    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] [Range(0, 0.5f)] private float _mouseSmoothTime = 0.03f;
    [SerializeField] private float _minCameraClamp;
    [SerializeField] private float _maxCameraClamp;

    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;
    private float _xRotation;

    private Camera _cam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        CameraLookRotationHandler();
        _cam.transform.localPosition = _cameraHolder.position;
    }

    private void CameraLookRotationHandler()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Smooth look rotations.
        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);

        // Setting raw values with mouse sensitivity.
        _xRotation -= _currentMouseDelta.y * _mouseSensitivity;

        // Prevents object from breaking its neck.
        _xRotation = Mathf.Clamp(_xRotation, _minCameraClamp, _maxCameraClamp);

        // Move the camera
        transform.Rotate(_currentMouseDelta.x * _mouseSensitivity * Vector3.up);
        _cam.transform.localEulerAngles = new Vector2(_xRotation, transform.eulerAngles.y);
    }
}