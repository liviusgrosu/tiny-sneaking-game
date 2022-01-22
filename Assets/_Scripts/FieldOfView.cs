using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float FarRadius;

    public float NearRadiusMin;
    
    [HideInInspector] public float NearRadiusCurrent;
    [Range(0,360)] public float angle;

    public void Initialize()
    {
        // Setup variables
        NearRadiusCurrent = NearRadiusMin;
    }
}