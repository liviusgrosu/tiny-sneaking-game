using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 3.0f;
    [SerializeField] private float _distanceFromTarget = 3.0f;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private Transform _target;
    [SerializeField] private LayerMask _cameraShiftMask;
    [SerializeField] private Transform _cameraShiftPivot;
    [SerializeField] private float _cameraShiftMax = 15.0f;
    [SerializeField] private Transform _cameraOriginPoint;

    private float _rotationY;
    private bool _shiftingCamera;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private Vector3 _currentFinalCameraPos;

    private float _step;
    [SerializeField] private float _movementTime = 1.0f;
    private bool _goingBack;

    void Awake()
    {
        // Store current rotation of player
        _currentRotation = transform.localEulerAngles;
    }

    void Update()
    {
        // Drag the camera around the player object
        if (Input.GetMouseButton(1) && !_shiftingCamera)
        {
            // Get rotation in relation to mouse movement
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            _rotationY += mouseX;
            Vector3 nextRotation = new Vector3(45.0f, _rotationY, 0.0f);

            // Apply a smooth damp filter to make rotation movement fluid
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _shiftingCamera = true;
            // Get mouse ray direction
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit, 3000f, _cameraShiftMask))
            {
                // Shift back to the original position
                transform.position = _cameraOriginPoint.position;

                // Get direction of mouse ray to point
                float distToPoint = Vector3.Magnitude(_cameraOriginPoint.position - hit.point);
                Vector3 dirMouse = mouseRay.direction * distToPoint;

                // Get direction from pivot to mouse ray target
                Vector3 dirToMouseFromPivot = (transform.position + dirMouse) - _cameraShiftPivot.position;
                dirToMouseFromPivot = new Vector3(dirToMouseFromPivot.x, 0f, dirToMouseFromPivot.z);
                dirToMouseFromPivot = Vector3.ClampMagnitude(dirToMouseFromPivot, _cameraShiftMax);

                // Move the camera to the final position    
                Vector3 finalCameraPos = _cameraOriginPoint.position + dirToMouseFromPivot;
                if (Vector3.Distance(transform.position, finalCameraPos) >= 0.1f)
                {
                    // Lerp the camera to the target position
                    transform.position = Vector3.Lerp(transform.position, finalCameraPos, _step / _movementTime);
                    if (_step < _movementTime)
                    {
                        _step += Time.deltaTime;
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            // Start the lerping process when going back to the original camera position
            _goingBack = true;
            _step = 0.0f;
        }
        
        if (_goingBack)
        {
            // Lerp back to the original camera position
            transform.position = Vector3.Lerp(transform.position, _cameraOriginPoint.position, _step / _movementTime);
            _step += Time.deltaTime;
            if (_step >= _movementTime)
            {
                // Once back, lock the camera to the player
                _shiftingCamera = false;
                _goingBack = false;
                _step = 0.0f;
            }
        }

        if (!_shiftingCamera)
        {
            // Apply calculation to cameras position 
            transform.position = _target.position - transform.forward * _distanceFromTarget;
        }

        _cameraOriginPoint.position = _target.position - transform.forward * _distanceFromTarget;
    }
}