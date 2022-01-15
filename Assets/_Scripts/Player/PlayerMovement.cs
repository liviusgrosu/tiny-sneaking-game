using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform DirectionCameraOffset;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.2f;
    
    [Header("Movement")]
    public float SneakSpeed = 3f;
    public float WalkingSpeed = 5f;
    private float _currentSpeed;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _currentSpeed = WalkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        // Move character relative to the camera rotation offset
        movementDirection += DirectionCameraOffset.forward * Input.GetAxisRaw("Vertical");
        movementDirection += DirectionCameraOffset.right * Input.GetAxisRaw("Horizontal");

        // Eliminate double speed with multiple inputs
        movementDirection = movementDirection.normalized;

        // Rotate the player based on their movement
        if (movementDirection != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            
            // Change movement to sneaking
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _currentSpeed = SneakSpeed; 
            }
            else
            {
                _currentSpeed = WalkingSpeed;
            }
            
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
