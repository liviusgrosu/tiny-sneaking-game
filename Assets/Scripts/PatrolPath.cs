using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    private List<Transform> _patrolPoints;
    void Awake()
    {
        _patrolPoints = new List<Transform>();

        foreach(Transform point in transform)
        {
            _patrolPoints.Add(point);
        }
    }

    void Update()
    {
        for (int i = 0; i < _patrolPoints.Count; i++)
        {
            Vector3 startPoint = _patrolPoints[i].position;
            Vector3 endPoint = _patrolPoints[(i + 1) % _patrolPoints.Count].position;
            Debug.DrawLine(_patrolPoints[i].position, endPoint, Color.red);
        }
    }

    public Transform GetPointPos(int index)
    {
        return _patrolPoints[index];
    }

    public int GetPatrolCount()
    {
        return _patrolPoints.Count;
    }
}