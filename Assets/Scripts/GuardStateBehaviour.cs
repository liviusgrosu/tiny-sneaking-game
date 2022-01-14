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
    public float TurningSpeed;
    public int AttackingDamage;

    private enum State
    {
        Patrol,
        Sighting,
        Search,
        Alert
    } 


    public float SightTime = 3.0f, NonSightTime = 3.0f;
    private float _currentSightTime, _currentNonSightTime;

    public float SearchTime = 6.0f;
    private float _currentSearchTime;

    private Vector3 _leftDirection, _rightDirection;

    private State _currentState;
    public bool EnablePathing;

    private Vector3 _targetDir;


    public float ScanningTime = 2.0f;
    private float _scanningTime;

    public Material PatrolMat, SightingMat, SearchMat, AlertMat;
    private MeshRenderer _mesh;


    public float AttackSpeed = 1.0f;
    private bool _attackingPlayer;

    void Awake()
    {
        _currentState = State.Patrol;
        _agent = GetComponent<NavMeshAgent>();
        _fov = GetComponent<FieldOfView>();
        _mesh = GetComponent<MeshRenderer>();
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
            if (EnablePathing && Vector3.Distance(transform.position, _currentTarget.position) <= 0.1f)
            {
                _currentPatrolPoint = (_currentPatrolPoint + 1) % PatrolPath.GetPatrolCount();
                GoToNextPoint();
            }

            // If entity can see the player
            if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Far)
            {
                // Start increasing the FOV
                _currentTarget = _fov.GetLastSighting();
                
                // Stop the agent
                _agent.isStopped = true;
                _currentState = State.Sighting;
                _mesh.material = SightingMat;

                // Setup variables for sighting
                _currentSightTime = 0f;
                _currentNonSightTime = 0f;
            }
            else if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Near)
            {
                // Go into alert when player is seen clearly
                _fov.ToggleNearFOV(_fov.FarRadius);
                _currentTarget = _fov.GetLastSighting();
                _currentState = State.Alert;
                _mesh.material = AlertMat;
            }
        }

        else if (_currentState == State.Sighting)
        {
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
                    if (_currentSightTime >= SightTime)
                    {
                        _currentState = State.Search;
                        _mesh.material = SearchMat;
                        _agent.isStopped = false;
                    }
                }
                else if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Near)
                {
                    _agent.isStopped = false;
                    // Go into alert when player is seen clearly
                    _fov.ToggleNearFOV(_fov.FarRadius);
                    _currentTarget = _fov.GetLastSighting();
                    _currentState = State.Alert;
                    _mesh.material = AlertMat;
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
                    _fov.ToggleNearFOV(_fov.NearRadiusMin);
                    _currentState = State.Patrol;
                    _mesh.material = PatrolMat;
                }
            }

            if (Vector3.Angle(_targetDir, transform.forward) > 0.1f)
            {
                // Look at the player
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime * TurningSpeed, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        else if (_currentState == State.Search)
        {
            // TODO get rid of this bool check, we can just use the near and far regions
            if (_fov.CanSeePlayer)
            {
                if (_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Far)
                {
                    // Go towards the player
                    _currentTarget = _fov.GetLastSighting();
                    _agent.destination = _currentTarget.position;

                    // Setup scanning directions
                    _rightDirection = transform.right;
                    _leftDirection = -transform.right;

                    _targetDir = _rightDirection;
                    _scanningTime = 0f;
                    _currentSearchTime = 0f;
                    _agent.isStopped = false;
                }
                else if(_fov.CurrentFOVRegion == FieldOfView.FOVRegion.Near)
                {
                    _agent.isStopped = false;
                    // Go into alert when player is seen clearly
                    _fov.ToggleNearFOV(_fov.FarRadius);
                    _currentTarget = _fov.GetLastSighting();
                    _currentState = State.Alert;
                    _mesh.material = AlertMat;
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
                        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _targetDir, Time.deltaTime * TurningSpeed, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);
                        _scanningTime = 0f;
                    }
                    else
                    {
                        // Change directions when the other has been scanned
                        _scanningTime += Time.deltaTime;

                        if (_scanningTime >= ScanningTime)
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

                    _currentSearchTime += Time.deltaTime;

                    // Go back to patrolling when no player is found
                    if (_currentSearchTime >= SearchTime)
                    {
                        _agent.isStopped = false;
                        _currentTarget = _oldTarget;
                        _agent.destination = _currentTarget.position;
                        _fov.ToggleNearFOV(_fov.NearRadiusMin);
                        _currentState = State.Patrol;
                        _mesh.material = PatrolMat;
                    }   
                }
            }
        }

        else if (_currentState == State.Alert)
        {
            if (_fov.CanSeePlayer)
            {
                _agent.destination = _currentTarget.position;
                if (Vector3.Distance(transform.position, _currentTarget.position) < 2f)
                {
                    _agent.isStopped = true;
                    if (!_attackingPlayer)
                    {
                        StartCoroutine(AttackPlayer());
                    }
                }
                else
                {
                    _agent.isStopped = false;
                    _attackingPlayer = false;
                    StopCoroutine(AttackPlayer());
                }
            }
            else
            {
                _agent.isStopped = false;
                // Setup scanning directions
                _rightDirection = transform.right;
                _leftDirection = -transform.right;

                _targetDir = _rightDirection;
                _scanningTime = 0f;
                _currentSearchTime = 0f;

                _currentState = State.Search;
                _mesh.material = SearchMat;
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

    IEnumerator AttackPlayer()
    {
        _attackingPlayer = true;
        _currentTarget.GetComponent<PlayerHealth>().ReduceHealth(AttackingDamage);
        yield return new WaitForSeconds(AttackSpeed);
        _attackingPlayer = false;
    }
}