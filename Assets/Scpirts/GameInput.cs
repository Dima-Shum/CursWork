using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{

    private PlayerInputActions PlayerInputActions;

    public static GameInput instance {  get; private set; }

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        instance = this;
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();
        PlayerInputActions.combat.Attack.started += PlayerAttack_started;
    }


    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVector()
    {
        Vector2 InputVector = PlayerInputActions.Player.Move.ReadValue<Vector2>();

        return InputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 MousePos = Mouse.current.position.ReadValue();
        return MousePos;
    }
}
