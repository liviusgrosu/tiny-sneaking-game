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
    private float _currentSpeed;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _currentSpeed = _walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
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
            _currentSpeed = _walkingSpeed;
            
            // Apply speed to rigidbody velocity
            rb.velocity = movementDirection * _currentSpeed;
        }
        else 
        {
            // Stop moving
            rb.velocity = Vector3.zero;
        }
    }
}
