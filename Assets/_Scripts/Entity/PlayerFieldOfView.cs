using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldOfView : MonoBehaviour
{
    [HideInInspector] public FieldOfView FOV;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;

    private List<GameObject> _visibleObjects;

    // Start is called before the first frame update
    void Start()
    {
        _visibleObjects = new List<GameObject>();
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
        
        if (rangeChecks.Length != 0)
        {
            foreach(Collider col in rangeChecks)
            {
                // Get distance to object
                Transform target = col.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                // Check if object is within the viewing angle
                if (Vector3.Angle(transform.forward, directionToTarget) < FOV.angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    // If not obstructions present then the object is clearly visible to the player
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                    {
                        // Reveal that object
                        if (!_visibleObjects.Contains(col.gameObject))
                        {
                            _visibleObjects.Add(col.gameObject);
                            col.gameObject.GetComponent<Hideable>().ToggleState(true);
                        }
                    }
                }
            }
        }
    }
}
