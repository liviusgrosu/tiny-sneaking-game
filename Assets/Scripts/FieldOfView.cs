using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float NearRadius;
    public float FarRadius;

    [Range(0,360)]
    public float angle;

    public GameObject PlayerRef;

    public LayerMask TargetMask;
    public LayerMask ObstructionMask;

    public bool CanSeePlayer;

    // 0 - Not in sight
    // 1 - Far sight
    // 2 - Near sight
    public int FOVRegion;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, FarRadius, TargetMask);

        // Check if anything collides with the guards sphere
        if (rangeChecks.Length != 0)
        {
            // Get distance to player
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if player is within the viewing angle
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // If not obstructions present then the enemy is looking at the player
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                {
                    Debug.Log($"{distanceToTarget}, near: {NearRadius}, far: {FarRadius}");
                    
                    // Player is close
                    if (distanceToTarget <= NearRadius)
                    {
                        CanSeePlayer = true;
                        FOVRegion = 2;
                    }
                    // Player is far
                    else
                    {
                        CanSeePlayer = true;
                        FOVRegion = 1;
                    }
                }
                // Can't see the player
                else
                {
                    CanSeePlayer = false;
                    FOVRegion = 0;
                }
            }
            // Can't see the player
            else
            {
                CanSeePlayer = false;
                FOVRegion = 0;
            }
        }
        // Player not colliding with enemies attention sphere
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
            FOVRegion = 0;
        }
    }
}