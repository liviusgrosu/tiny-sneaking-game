using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldOfView : MonoBehaviour
{
    [HideInInspector] public FieldOfView FOV;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;

    // Start is called before the first frame update
    void Start()
    {
        FOV = GetComponent<FieldOfView>();
    }
}
