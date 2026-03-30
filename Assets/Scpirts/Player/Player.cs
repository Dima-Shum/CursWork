using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance {  get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [Header("Main settings")]

    [SerializeField] public int _maxHealth = 10;

    [SerializeField] private float _damageRecoveryTime = 0.5f;

    [SerializeField] private float _movingSpeed = 5f;

    [Header("Dash settings")]

    [SerializeField] private float _dashTime = 0.2f;

    [SerializeField] private int _dashSpeed = 4;

    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private float _dashCoolDownTime = 0.25f;

    private GameObject _losePanel;

    private float _MinMovingSpeed = 0.1f;

    private Rigidbody2D _rb;

    private KnockBack _knockBack;

    Vector2 inputVector;

    private bool _isRunning = false;

    public int _currentHealth;

    private bool _canTakeDamage;

    private bool _isAlive;

    private bool _isDashing;

    private float _initialMovingSpeed;

    private void Awake()
    {
        instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();  
        _initialMovingSpeed = _movingSpeed;
    }

    private void Start()
    {
        _isAlive = true;
        _currentHealth = _maxHealth; 
        _canTakeDamage = true;
        GameInput.instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.instance.OnPlayerDash += Player_OnPlayerDash;

        _losePanel = GameObject.Find("losePanel");
        if (_losePanel != null)
        {
            _losePanel.SetActive(false);
            Debug.Log("ѕанель поражени€ скрыта при старте");
        }
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
            ShowLose();


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

    private void Player_OnPlayerDash(object sender, EventArgs e)
    {
        Dash();
    }

    private void Dash()
    {
        if(!_isDashing) 
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        _movingSpeed *= _dashSpeed;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(_dashTime);

        trailRenderer.emitting = false;
        _movingSpeed = _initialMovingSpeed;

        yield return new WaitForSeconds(_dashCoolDownTime);
        _isDashing = false;
        
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

    private void OnDestroy()
    {
        GameInput.instance.OnPlayerAttack -= Player_OnPlayerAttack;
    }

    private void ShowLose()
    {
        if (_losePanel != null)
        {
            _losePanel.SetActive(true);
            StartCoroutine(ReturnToMenu());
        }
        else
        {
            Debug.LogError("—сылка на losePanel потер€на!");
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}