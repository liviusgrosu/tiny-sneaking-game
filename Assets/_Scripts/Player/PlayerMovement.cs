using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform _directionCameraOffset;
    private float turnSmoothVelocity;
    [SerializeField] private float _turnSmoothTime = 0.08f;
    
    [Header("Movement")]
    [SerializeField] private float _sneakSpeed = 1f;
    [SerializeField] private float _walkingSpeed = 4f;
    [SerializeField] private float _runningSpeed = 4f;
    private float _currentSpeed;
    private Rigidbody _rb;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _currentSpeed = _walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Don't allow the player to move after death
        if (GetComponent<PlayerHealth>().IsDead())
        {
            return; 
        }

        Vector3 movementDirection = Vector3.zero;

        // Move character relative to the camera rotation offset
        movementDirection += _directionCameraOffset.forward * Input.GetAxisRaw("Vertical");
        movementDirection += _directionCameraOffset.right * Input.GetAxisRaw("Horizontal");

        // Eliminate double speed with multiple inputs
        movementDirection = movementDirection.normalized;

        // Rotate the player based on their movement
        if (movementDirection != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, _turnSmoothTime);
            
            // TODO: Add other speeds like sneaking and running            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _currentSpeed = _runningSpeed;
            }
            else
            {
                _currentSpeed = _walkingSpeed;
            }

            // Apply speed to rigidbody velocity
            _rb.velocity = movementDirection * _currentSpeed;
        }
        else 
        {
            // Stop moving
            _rb.velocity = Vector3.zero;
        }
        
        _animator.SetFloat("speedPercent", _rb.velocity.magnitude / _runningSpeed);
    }
}
