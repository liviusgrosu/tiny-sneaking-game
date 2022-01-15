using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PatrolPath : MonoBehaviour
{
    private List<Transform> _patrolPoints;
    void Awake()
    {
        _patrolPoints = new List<Transform>();

        // Add each children into the list as points to the patrol
        foreach(Transform point in transform)
        {
            _patrolPoints.Add(point);
        }
    }

    void Update()
    {
        // Draw a line from each patrol point
        for (int i = 0; i < _patrolPoints.Count; i++)
        {
            Vector3 startPoint = _patrolPoints[i].position;
            Vector3 endPoint = _patrolPoints[(i + 1) % _patrolPoints.Count].position;
            Debug.DrawLine(_patrolPoints[i].position, endPoint, Color.red);
        }
    }

    public Transform GetPointPos(int index)
    {
        // Return a points position
        return _patrolPoints[index];
    }

    public int GetPatrolCount()
    {
        // Return length of patrol
        return _patrolPoints.Count;
    }
}