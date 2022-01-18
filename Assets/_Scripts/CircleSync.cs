using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeD = Shader.PropertyToID("_Size");

    [SerializeField] private Material[] _seeThroughMaterials;
    [SerializeField] private LayerMask _mask;

    [SerializeField] private float _maxSize = 1.0f;
    [Tooltip("How long it takes to transition. The smaller the faster")]
    [SerializeField] private float _cutoutTime = 1.0f;
    private float _currentCutoutTime;
    private float _currentSize;
    [SerializeField] private Transform _targetDir;


    void Update()
    {
        Vector3 dir = Camera.main.transform.position - _targetDir.position;
        Ray ray = new Ray(_targetDir.position, dir.normalized);

        // If wall is in the way then change its material
        if (Physics.Raycast(ray, 3000, _mask) && _currentSize == 0.0f)
        {
            _currentCutoutTime = 0f;
            StopAllCoroutines();
            StartCoroutine(EnableCutout());
        }
        // Close the circle if no obstacle is present
        else if (!Physics.Raycast(ray, 3000, _mask) && _currentSize == _mask)
        {
            _currentCutoutTime = 0f;
            StopAllCoroutines();
            StartCoroutine(DisableCutout());
        }

        // Apply the shaders position to middle of the player
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        SetSeeThroughShaderPos(view);
    }

    IEnumerator EnableCutout()
    {
        // Open the cutout circle by making it bigger
        while (_currentCutoutTime < _cutoutTime)
        {
            _currentCutoutTime += Time.deltaTime;
            _currentSize = Mathf.Clamp01(_currentCutoutTime / _cutoutTime) * _maxSize;
            SetSeeThroughShaderSize(_currentSize);
            yield return null;
        }
        _currentSize = _mask;
    }

    IEnumerator DisableCutout()
    {
        // Close the cutout circle by making it smaller
        while (_currentCutoutTime < _cutoutTime)
        {
            _currentCutoutTime += Time.deltaTime;
            _currentSize = (1 - Mathf.Clamp01(_currentCutoutTime / _cutoutTime)) * _maxSize;
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
        foreach (Material mat in _seeThroughMaterials)
        {
            mat.SetFloat(SizeD, value);
        }
    }

    void SetSeeThroughShaderPos(Vector3 value)
    {
        foreach (Material mat in _seeThroughMaterials)
        {
            mat.SetVector(PosID, value);
        }
    }

}
