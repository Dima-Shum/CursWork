using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using CursWork.Utils;
using System;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 0.5f;
    private float _nextAttackTime = 0f;

    private Rigidbody2D _rb;


    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    public event EventHandler OnEnemyAttack;



    public EnemyAI(State state)
    {
        _currentState = state;
    }



    public enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death

    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _rb = GetComponent<Rigidbody2D>();

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }

    private void Update()
    {

        StateHandler();
        MovementDirectionHandler();
    }
    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
        _rb.linearVelocity = Vector2.zero; 
        _rb.angularVelocity = 0f;
    }
    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();

                break;

            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;

            case State.Death:
                break;

            default:
            case State.Idle:
                break;
        }
    }



    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning())
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);
        State newState = State.Roaming;
        if (_isChasingEnemy)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (_isAttackingEnemy)
            if (distanceToPlayer <= _attackingDistance)
            {
                if (Player.instance.IsAlive())
                    newState = State.Attacking;

                else
                    newState = State.Roaming;
            }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }

            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;

            }

            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }
            _currentState = newState;
        }
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.instance.transform.position);
    }

    public bool IsRunning()
    {
        if (_navMeshAgent.velocity == Vector3.zero)
            return false;
        else
        {
            return true;
        }
    }
    private void Roaming()
    {
        _startingPosition = transform.position; // дл€ того, чтобы враги могли ходить в любую часть сцены. ѕотом, возможно, убрать
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + CursWorkUtils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        transform.rotation = sourcePosition.x > targetPosition.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
