using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{

    private PlayerInputActions _PlayerInputActions;

    public static GameInput instance {  get; private set; }

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        instance = this;
        _PlayerInputActions = new PlayerInputActions();
        _PlayerInputActions.Enable();
        _PlayerInputActions.combat.Attack.started += PlayerAttack_started;
    }


   
    public Vector2 GetMovementVector()
    {
        Vector2 InputVector = _PlayerInputActions.Player.Move.ReadValue<Vector2>();

        return InputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 MousePos = Mouse.current.position.ReadValue();
        return MousePos;
    }

    public void DisableMovement()
    {
        _PlayerInputActions.Disable();
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {

        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
}
