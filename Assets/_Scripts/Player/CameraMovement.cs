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
    [SerializeField] private Transform _cameraOriginPoint;

    private float _rotationY;
    private bool _shiftingCamera;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    void Awake()
    {
        // Store current rotation of player
        _currentRotation = transform.localEulerAngles;
    }

    void Update()
    {
        // Drag the camera around the player object
        if (Input.GetMouseButton(1))
        {
            // TODO: move the camera back when its shifted

            // Get rotation in relation to mouse movement
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            _rotationY += mouseX;
            Vector3 nextRotation = new Vector3(45f, _rotationY, 0);

            // Apply a smooth damp filter to make rotation movement floaty
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _shiftingCamera = true;
            // TODO: Focus camera where mouse is pointing
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

                // Move the camera to the final position    
                Vector3 finalCameraDir = _cameraOriginPoint.position + dirToMouseFromPivot;
                transform.position = finalCameraDir;

                Debug.DrawRay(_cameraShiftPivot.position, dirToMouseFromPivot, Color.red);
                Debug.DrawRay(transform.position, dirMouse, Color.green);
                Debug.DrawRay(transform.position, dirToMouseFromPivot, Color.blue);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _shiftingCamera = false;
            transform.position = _cameraOriginPoint.position;
        }
        
        if (!_shiftingCamera)
        {
            // Apply calculation to cameras position 
            transform.position = _target.position - transform.forward * _distanceFromTarget;
        }
    }

    IEnumerator PerformRotate(Vector3 nextRotation)
    {
        while (Vector3.Distance(_currentRotation, nextRotation) > 1.0f)
        {
            // Apply a smooth damp filter to make rotation movement floaty
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;
            yield return null;
        }
    }
}