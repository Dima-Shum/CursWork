using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance {  get; private set; }

    private Rigidbody2D rb;


    private float movingSpeed = 5f;

     Vector2 inputVector;

    private float MinMovingSpeed = 0.1f;
    private bool isRunning = false;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();

        
    }

    private void Start()
    {
        GameInput.instance.OnPlayerAttack += Player_OnPlayerAttack;
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
       HandleMovement();
    }

    private void HandleMovement()
    {
        

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if(Mathf.Abs(inputVector.x) > MinMovingSpeed || Mathf.Abs(inputVector.y) > MinMovingSpeed)
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }      
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 PlayerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);  
        return PlayerScreenPosition;    
    }
}