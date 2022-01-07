using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeD = Shader.PropertyToID("_Size");

    public Material WallMaterial;
    public LayerMask Mask;

    void Update()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);

        if (Physics.Raycast(ray, 3000, Mask))
        {
            WallMaterial.SetFloat(SizeD, 1);
        }
        else
        {
            WallMaterial.SetFloat(SizeD, 0);
        }
    }
}
