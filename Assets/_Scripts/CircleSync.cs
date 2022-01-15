﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeD = Shader.PropertyToID("_Size");

    public Material[] SeeThroughMaterials;
    public LayerMask Mask;

    public float MaxSize = 1.0f;
    [Tooltip("How long it takes to transition. The smaller the faster")]
    public float CutoutTime = 1.0f;
    private float _currentCutoutTime;
    private float _currentSize;
    public Transform TargetDir;


    void Update()
    {
        Vector3 dir = Camera.main.transform.position - TargetDir.position;
        Ray ray = new Ray(TargetDir.position, dir.normalized);

        // If wall is in the way then change its material
        if (Physics.Raycast(ray, 3000, Mask) && _currentSize == 0.0f)
        {
            _currentCutoutTime = 0f;
            StopAllCoroutines();
            StartCoroutine(EnableCutout());
        }
        // Close the circle if no obstacle is present
        else if (!Physics.Raycast(ray, 3000, Mask) && _currentSize == MaxSize)
        {
            _currentCutoutTime = 0f;
            StopAllCoroutines();
            StartCoroutine(DisableCutout());
        }

        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        SetSeeThroughShaderPos(view);
    }

    IEnumerator EnableCutout()
    {
        // Open the cutout circle by making it bigger
        while (_currentCutoutTime < CutoutTime)
        {
            _currentCutoutTime += Time.deltaTime;
            _currentSize = Mathf.Clamp01(_currentCutoutTime / CutoutTime) * MaxSize;
            SetSeeThroughShaderSize(_currentSize);
            yield return null;
        }
        _currentSize = MaxSize;
    }

    IEnumerator DisableCutout()
    {
        // Close the cutout circle by making it smaller
        while (_currentCutoutTime < CutoutTime)
        {
            _currentCutoutTime += Time.deltaTime;
            _currentSize = (1 - Mathf.Clamp01(_currentCutoutTime / CutoutTime)) * MaxSize;
            SetSeeThroughShaderSize(_currentSize);
            yield return null;
        }
        _currentSize = 0.0f;
    }

    void OnApplicationQuit()
    {
        // Reset the shader material when application is shutdown
        SetSeeThroughShaderSize(0);
    }

    void SetSeeThroughShaderSize(float value)
    {
        foreach (Material mat in SeeThroughMaterials)
        {
            mat.SetFloat(SizeD, value);
        }
    }

    void SetSeeThroughShaderPos(Vector3 value)
    {
        foreach (Material mat in SeeThroughMaterials)
        {
            mat.SetVector(PosID, value);
        }
    }

}