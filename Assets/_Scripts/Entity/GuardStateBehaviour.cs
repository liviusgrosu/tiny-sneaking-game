using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PatrolPath _patrolPath;
    private Transform _currentTarget;
    private Transform _oldTarget;
    private FieldOfView _fov;
    private int _currentPatrolPoint;
    private NavMeshAgent _agent;
    [SerializeField] private float _turningSpeed;
    [SerializeField] private int _attackingDamage;
    private enum State
    {
        Patrol,
        Sighting,
        Search,
        Alert
    } 
    [SerializeField] private float _sightTime = 3.0f, _nonSightTime = 3.0f;
    private float _currentSightTime, _currentNonSightTime;
    [SerializeField] private float _searchTime = 6.0f;
    private float _currentSearchTime;
    private Vector3 _leftDirection, _rightDirection;
    private State _currentState;
    [SerializeField] private bool _enablePathing;
    private Vector3 _targetDir;
    [SerializeField] private float _scanningTime = 2.0f;
    private float _scanningCurrentTime;
    private MeshRenderer _mesh;
    private Animator _animator;
    [SerializeField] private float _attackSpeed = 1.0f;
    private bool _attackingPlayer;

    void Awake()
    {
        _currentState = State.Patrol;
        _agent = GetComponent<NavMeshAgent>();
        _fov = GetComponent<FieldOfView>();
        _mesh = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Start pathing to the first patrol point
        if (_enablePathing)
        {
            GoToNextPoint();
        }
    }

    void Update()
    {
        // Go into alert phase when player is clearly seen
        if (_currentState != State.Alert && _fov.CurrentFOVRegion == FieldOfView.FOVRegion.Near)
        {
            _agent.isStopped = false;
            
            _fov.ToggleNearFOV(_fov.FarRadius);
            _currentTarget = _gameManager.PlayerInstance;
            _currentState = State.Alert;
        }

        /* 
        ---------------
        Patrol State
        ---------------
        */ 
        if (_currentState == State.Patrol)
        {
            // If arriving to the destination patrol point, then go to the next one
            if (_enablePathing && Vector3.Distance(transform.position, _agent.destination) <= 0.1f)
            {
                _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPath.GetPatrolCount();
                GoToNextPoint();
            }

            // If entity can see the player
            if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Far)
            {
                // Start increasing the FOV
                _currentTarget = _gameManager.PlayerInstance;
                
                // Stop the agent
                _agent.isStopped = true;
                _currentState = State.Sighting;

                // Setup variables for sighting
                _currentSightTime = 0f;
                _currentNonSightTime = 0f;
            }
        }

        /* 
        ---------------
        Sighting State
        ---------------
        */ 
        else if (_currentState == State.Sighting)
        {
            // Prepare to go into sighting state
            if (_fov.CanSeePlayer)
            {
                if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Far)
                {
                    // Provide the guard with a target to look at
                    _targetDir = _currentTarget.position - transform.position;

                    // Increase sighting time
                    _currentNonSightTime = 0f;
                    _currentSightTime += Time.deltaTime;
                    // Go into searching stage if player stays in the sighting range
                    if (_currentSightTime >= _sightTime)
                    {
                        _currentState = State.Search;
                        _agent.isStopped = false;
                    }
                }
            }
            // Prepare to go back into patrolling phase
            else
            {
                // Increase the non sighting time when player is out of guards FOV
                _currentSightTime = 0.0f;
                _currentNonSightTime += Time.deltaTime;

                if (_currentNonSightTime >= _nonSightTime)
                {
                    // Go back to patroling if guard doesnt see the player anymore
                    _agent.isStopped = false;
                    _currentTarget = _oldTarget;
                    _fov.ToggleNearFOV(_fov.NearRadiusMin);
                    _currentState = State.Patrol;
                }
            }

            if (Vector3.Angle(_targetDir, transform.forward) > 0.1f)
            {
                // Look at the player
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime * _turningSpeed, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        /* 
        ---------------
        Searching State
        ---------------
        */ 
        else if (_currentState == State.Search)
        {
            // TODO get rid of this bool check, we can just use the near and far regions
            if (_fov.CanSeePlayer)
            {
                if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Far)
                {
                    // Go towards the player
                    _currentTarget = _gameManager.PlayerInstance;
                    _agent.destination = _currentTarget.position;

                    // Setup scanning directions
                    _rightDirection = transform.right;
                    _leftDirection = -transform.right;

                    // Setup variables for when the guard looses sight on player
                    _targetDir = _rightDirection;
                    _scanningCurrentTime = 0f;
                    _currentSearchTime = 0f;
                    _agent.isStopped = false;
                }
            }
            else
            {
                if (ArrivedAtTargetLocation(_agent.destination))
                {
                    // Start looking left and right
                    _agent.isStopped = true;
                    if (Vector3.Angle(_targetDir, transform.forward) > 1f)
                    {
                        // Look at the new direction
                        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime * _turningSpeed, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);
                        _scanningCurrentTime = 0f;
                    }
                    else
                    {
                        // Change directions when the other direction has been scanned
                        _scanningCurrentTime += Time.deltaTime;

                        if (_scanningCurrentTime >= _scanningTime)
                        {
                            if (_targetDir == _rightDirection)
                            {
                                // Look left now
                                _targetDir = _leftDirection;
                            }
                            else
                            {
                                // Look right now
                                _targetDir = _rightDirection;
                            }
                        }                        
                    }

                    // Go back to patrolling when no player is found
                    _currentSearchTime += Time.deltaTime;
                    if (_currentSearchTime >= _searchTime)
                    {
                        _agent.isStopped = false;
                        _currentTarget = _oldTarget;
                        _agent.destination = _currentTarget.position;
                        _fov.ToggleNearFOV(_fov.NearRadiusMin);
                        _currentState = State.Patrol;
                    }   
                }
            }
        }

        /* 
        ---------------
        Alert State
        ---------------
        */ 
        else if (_currentState == State.Alert)
        {
            if (_fov.CanSeePlayer)
            {
                // Chase the player when they are visible
                _agent.destination = _currentTarget.position;
                if (Vector3.Distance(transform.position, _currentTarget.position) < 2f)
                {
                    // Start attacking the player when theyre close
                    _agent.isStopped = true;
                    if (!_attackingPlayer)
                    {
                        StartCoroutine(AttackPlayer());
                    }
                }
                else
                {
                    // Stop attacking the player when they get too far
                    _agent.isStopped = false;
                    _attackingPlayer = false;
                    StopCoroutine(AttackPlayer());
                }
            }
            else
            {
                // Go back to scanning phase when guard looses sighting of player
                _agent.isStopped = false;
                
                // Setup variables for sighting phase
                _rightDirection = transform.right;
                _leftDirection = -transform.right;

                _targetDir = _rightDirection;
                _scanningCurrentTime = 0f;
                _currentSearchTime = 0f;

                _currentState = State.Search;
            }
        }
    }

    private void GoToNextPoint()
    {
        // Get next point of patrol
        _currentTarget = _patrolPath.GetPointPos(_currentPatrolPoint);
        _oldTarget = _currentTarget;
        _agent.destination = _currentTarget.position;
    }

    private bool ArrivedAtTargetLocation(Vector3 targetPos)
    {
        // Check if guard has arrived at next patrol location
        return Mathf.Abs(targetPos.x - transform.position.x) < 0.1f && Mathf.Abs(targetPos.z - transform.position.z) < 0.1f;
    }

    IEnumerator AttackPlayer()
    {
        // Reduce the players health when attacked
        _attackingPlayer = true;
        _currentTarget.GetComponent<PlayerHealth>().ReduceHealth(_attackingDamage);
        yield return new WaitForSeconds(_attackSpeed);
        _attackingPlayer = false;
    }
}