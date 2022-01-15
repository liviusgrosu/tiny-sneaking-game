using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 3.0f;
    [SerializeField] private float _distanceFromTarget = 3.0f;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private Transform _target;

    private float _rotationY;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    private bool _performingRotation;

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
            // Get rotation in relation to mouse movement
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            _rotationY += mouseX;
            Vector3 nextRotation = new Vector3(45f, _rotationY, 0);

            // Apply a smooth damp filter to make rotation movement floaty
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !_performingRotation)
        {
            Vector3 nextRotation = new Vector3(45f, transform.eulerAngles.y + 90f, 0);
            _performingRotation = true;
            StartCoroutine(PerformRotate(nextRotation));
        }

        // Apply calculation to cameras position 
        transform.position = _target.position - transform.forward * _distanceFromTarget;
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
        _performingRotation = false;
    }
}