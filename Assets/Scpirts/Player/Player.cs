using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance {  get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private int _maxHealth = 10;

    [SerializeField] private float _damageRecoveryTime = 0.5f;

    [SerializeField] private float _movingSpeed = 5f;

    private float _MinMovingSpeed = 0.1f;

    private Rigidbody2D _rb;

    private KnockBack _knockBack;

    Vector2 inputVector;

    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    private void Awake()
    {
        instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();   
    }

    private void Start()
    {
        _isAlive = true;
        _currentHealth = _maxHealth; 
        _canTakeDamage = true;
        GameInput.instance.OnPlayerAttack += Player_OnPlayerAttack;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if(_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = System.Math.Max(0, _currentHealth -= damage);
            Debug.Log(_currentHealth);
            _knockBack.GetKnockedBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive=false; 
            _knockBack.StopKnockBackMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            GameInput.instance.DisableMovement();


        }
           
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void Player_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        inputVector = GameInput.instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if(_knockBack.IsGettingKnockBack)
            return;

       HandleMovement();
    }


    private void HandleMovement()
    {
        

        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if(Mathf.Abs(inputVector.x) > _MinMovingSpeed || Mathf.Abs(inputVector.y) > _MinMovingSpeed)
        {
            _isRunning = true;
        }

        else
        {
            _isRunning = false;
        }      
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 PlayerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);  
        return PlayerScreenPosition;    
    }
}