using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateBehaviour : MonoBehaviour
{
    public PatrolPath PatrolPath;
    private Transform _currentTarget;
    private int _currentPatrolPoint;
    private NavMeshAgent _agent;

    private enum State
    {
        Patrol,
        Search,
        Alert
    } 

    private State _currentState;

    void Awake()
    {
        _currentState = State.Patrol;
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GoToNextPoint();
    }

    void Update()
    {
        if (_currentState == State.Patrol)
        {
            if (Vector3.Distance(transform.position, _currentTarget.position) <= 0.01f)
            {
                _currentPatrolPoint = (_currentPatrolPoint + 1) % PatrolPath.GetPatrolCount();
                GoToNextPoint();
            }
        }
    }

    private void GoToNextPoint()
    {
        _currentTarget = PatrolPath.GetPointPos(_currentPatrolPoint);
        _agent.destination = _currentTarget.position;
    }
}