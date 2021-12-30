using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateBehaviour : MonoBehaviour
{
    public PatrolPath PatrolPath;
    private Transform _currentTarget;
    private Transform _oldTarget;
    private FieldOfView _fov;
    private int _currentPatrolPoint;
    private NavMeshAgent _agent;

    private enum State
    {
        Patrol,
        Sighting,
        Search,
        Alert
    } 


    public float SightTime = 3.0f, NonSightTime = 3.0f;
    private float _currentSightTime, _currentNonSightTime;

    private State _currentState;
    public bool EnablePathing;

    private Vector3 _targetDir;

    void Awake()
    {
        _currentState = State.Patrol;
        _agent = GetComponent<NavMeshAgent>();
        _fov = GetComponent<FieldOfView>();
    }

    void Start()
    {
        if (EnablePathing)
        {
            GoToNextPoint();
        }
    }

    void Update()
    {
        if (_currentState == State.Patrol)
        {
            // Patrol the path
            if (EnablePathing && Vector3.Distance(transform.position, _currentTarget.position) <= 0.01f)
            {
                _currentPatrolPoint = (_currentPatrolPoint + 1) % PatrolPath.GetPatrolCount();
                GoToNextPoint();
            }

            // If entity can see the player
            if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Far)
            {
                StartCoroutine(_fov.IncreaseFOV());
                _currentTarget = _fov.GetLastSighting();
                
                _agent.isStopped = true;
                _currentState = State.Sighting;

                _currentSightTime = 0.0f;
                _currentNonSightTime = 0.0f;
            }
        }

        else if (_currentState == State.Sighting)
        {
            
            if (_fov.CanSeePlayer)
            {
                _targetDir = _currentTarget.position - transform.position;

                _currentNonSightTime = 0.0f;
                _currentSightTime += Time.deltaTime;
                if (_currentSightTime >= SightTime)
                {
                    //_currentState = State.Search;
                }
            }
            else
            {
                _currentSightTime = 0.0f;
                _currentNonSightTime += Time.deltaTime;
                if (_currentNonSightTime >= NonSightTime)
                {
                    _agent.isStopped = false;
                    _currentTarget = _oldTarget;
                    _fov.ResetFOV();
                    _currentState = State.Patrol;
                }
            }

            if (Vector3.Angle(_targetDir, transform.forward) > 0.1f)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }            

            // Wait a few seconds 
            // Go into search if player is still in far sight
            // If player leaves far sigthing before seconds pass then go back to patrolling
        }

        else if (_currentState == State.Search)
        {
            
        }
    }

    private void GoToNextPoint()
    {
        _currentTarget = PatrolPath.GetPointPos(_currentPatrolPoint);
        _oldTarget = _currentTarget;
        _agent.destination = _currentTarget.position;
    }
}