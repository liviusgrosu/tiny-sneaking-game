using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float FarRadius;

    public float NearRadiusMax, NearRadiusMin;
    
    [HideInInspector]
    public float NearRadiusCurrent;

    [Range(0,360)]
    public float angle;

    public GameObject PlayerRef;

    public LayerMask TargetMask;
    public LayerMask ObstructionMask;

    public bool CanSeePlayer;

    // 0 - Not in sight
    // 1 - Far sight
    // 2 - Near sight
    // public int FOVRegion;

    public enum FOVRegion
    {
        Near,
        Far,
        None
    };
    public FOVRegion CurrentFOVRegion;

    private void Awake()
    {
        NearRadiusCurrent = NearRadiusMin;
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
                    // Player is close
                    if (distanceToTarget <= NearRadiusCurrent)
                    {
                        CanSeePlayer = true;
                        CurrentFOVRegion = FOVRegion.Near;
                    }
                    // Player is far
                    else if (distanceToTarget > NearRadiusCurrent && distanceToTarget <= FarRadius)
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
        CanSeePlayer = false;
        CurrentFOVRegion = FOVRegion.None;
    }

    public IEnumerator IncreaseFOV()
    {
        while (NearRadiusCurrent < NearRadiusMax)
        {
            NearRadiusCurrent += Time.deltaTime;
            yield return null;
        }
    }

    public void ResetFOV()
    {
        NearRadiusCurrent = NearRadiusMin;
    }

    public Transform GetLastSighting()
    {
        return PlayerRef.transform;
    }
}