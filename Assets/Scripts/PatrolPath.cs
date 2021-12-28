using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> PatrolPoints;
    void Awake()
    {
        PatrolPoints = new List<Transform>();

        foreach(Transform point in transform)
        {
            PatrolPoints.Add(point);
        }
    }

    void Update()
    {
        for (int i = 0; i < PatrolPoints.Count; i++)
        {
            Vector3 startPoint = PatrolPoints[i].position;
            Vector3 endPoint = PatrolPoints[(i + 1) % PatrolPoints.Count].position;
            Debug.DrawLine(PatrolPoints[i].position, endPoint, Color.red);
        }
    }
}