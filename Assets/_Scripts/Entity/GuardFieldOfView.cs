using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFieldOfView : MonoBehaviour
{
    [HideInInspector] public FieldOfView FOV;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;

    [HideInInspector] public bool CanSeePlayer;

    public enum FOVRegion
    {
        Near,
        Far,
        None
    };
    [HideInInspector] public FOVRegion CurrentFOVRegion;

    private void Start()
    {
        // Setup data for guards field of view
        CurrentFOVRegion = FOVRegion.None;
        FOV = GetComponent<FieldOfView>();
        FOV.Initialize();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        // Routinely check for player in FOV  
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, FOV.FarRadius, _targetMask);

        // Check if anything collides with the guards sphere
        if (rangeChecks.Length != 0)
        {
            // Get distance to player
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if player is within the viewing angle
            if (Vector3.Angle(transform.forward, directionToTarget) < FOV.angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // If not obstructions present then the enemy is looking at the player
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    // Player is close
                    if (distanceToTarget <= FOV.NearRadiusCurrent)
                    {
                        CanSeePlayer = true;
                        CurrentFOVRegion = FOVRegion.Near;
                    }
                    // Player is far
                    else if (distanceToTarget > FOV.NearRadiusCurrent && distanceToTarget <= FOV.FarRadius)
                    {
                        CanSeePlayer = true;
                        CurrentFOVRegion = FOVRegion.Far;
                    }
                }
                // Can't see the player
                else
                {
                    ResetViewOfPlayer();
                }
            }
            // Can't see the player
            else
            {
                ResetViewOfPlayer();
            }
        }
        // Player not colliding with enemies attention sphere
        else if (CanSeePlayer)
        {
            ResetViewOfPlayer();
        }
    }

    private void ResetViewOfPlayer()
    {
        // Reset player interaction with FOV
        CanSeePlayer = false;
        CurrentFOVRegion = FOVRegion.None;
    }

    public void ToggleNearFOV(float target)
    {
        FOV.NearRadiusCurrent = target;
    }

    public void ResetFOV()
    {
        FOV.NearRadiusCurrent = FOV.NearRadiusMin;
    }
}