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

    private Vector3 _leftDirection, _rightDirection;

    private State _currentState;
    public bool EnablePathing;

    private Vector3 _targetDir;


    public float ScanningTimeMax = 2.0f;
    private float _scanningTime;

    public Transform LastPlayerLocation;

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
                // Start increasing the FOV
                StartCoroutine(_fov.IncreaseFOV());
                _currentTarget = _fov.GetLastSighting();
                
                // Stop the agent
                _agent.isStopped = true;
                _currentState = State.Sighting;

                // Setup variables for sighting
                _currentSightTime = 0f;
                _currentNonSightTime = 0f;
            }
        }

        else if (_currentState == State.Sighting)
        {
            if (_fov.CanSeePlayer)
            {
                // Provide the guard with a target to look at
                _targetDir = _currentTarget.position - transform.position;

                // Increase sighting time
                _currentNonSightTime = 0f;
                _currentSightTime += Time.deltaTime;
                // Go into searching stage if player stays in the sighting range
                if (_currentSightTime >= SightTime)
                {
                    _currentState = State.Search;
                    _agent.isStopped = false;
                }
            }
            else
            {
                // Increase the non sighting time when player is out of guards FOV
                _currentSightTime = 0.0f;
                _currentNonSightTime += Time.deltaTime;

                if (_currentNonSightTime >= NonSightTime)
                {
                    // Go back to patroling if guard doesnt see the player anymore
                    _agent.isStopped = false;
                    _currentTarget = _oldTarget;
                    _fov.ResetFOV();
                    _currentState = State.Patrol;
                }
            }

            if (Vector3.Angle(_targetDir, transform.forward) > 0.1f)
            {
                // Look at the player
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        else if (_currentState == State.Search)
        {
            /*
            * Go towards the last players position as long as theyre in the far range
            * If at last players position and no player found look left and right
            * Look forward after that and start a timer
            * After the timer is done, then go back to patrolling
            * 
            */

            if (_fov.CanSeePlayer)
            {
                _currentNonSightTime = 0f;
                _currentTarget = _fov.GetLastSighting();
                // --- TEMP ---
                LastPlayerLocation.position = _currentTarget.position;
                // ------------
                _agent.destination = _currentTarget.position;

                _rightDirection = transform.right;
                _leftDirection = -transform.right;

                _targetDir = _rightDirection;
                _scanningTime = 0f;
                _agent.isStopped = false;
            }
            else
            {
                if (ArrivedAtTargetLocation(_agent.destination))
                {
                    _agent.isStopped = true;
                    Debug.Log(Vector3.Angle(_targetDir, transform.forward));
                    // Look left and right
                    if (Vector3.Angle(_targetDir, transform.forward) > 1f)
                    {
                        // Look at the player
                        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime * 2f, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);
                        _scanningTime = 0f;
                    }
                    else
                    {
                        _scanningTime += Time.deltaTime;

                        if (_scanningTime >= ScanningTimeMax)
                        {
                            if (_targetDir == _rightDirection)
                            {
                                _targetDir = _leftDirection;
                            }
                            else
                            {
                                _targetDir = _rightDirection;
                            }
                        }                        
                    }



                    _currentNonSightTime += Time.deltaTime;
                    // Go back to patrolling when no player is found
                    if (_currentNonSightTime >= NonSightTime)
                    {
                        //_fov.ResetFOV();
                        //_currentState = State.Patrol;
                        //_currentTarget = _oldTarget;
                    }   
                }
            }

        }
    }

    private void GoToNextPoint()
    {
        _currentTarget = PatrolPath.GetPointPos(_currentPatrolPoint);
        _oldTarget = _currentTarget;
        _agent.destination = _currentTarget.position;
    }

    private bool ArrivedAtTargetLocation(Vector3 targetPos)
    {
        return Mathf.Abs(targetPos.x - transform.position.x) < 0.1f && Mathf.Abs(targetPos.z - transform.position.z) < 0.1f;
    }
}