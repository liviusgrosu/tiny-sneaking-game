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
        
        
    }
}
