using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{

    private PlayerInputActions _PlayerInputActions;

    public static GameInput instance {  get; private set; }

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash;

    private void Awake()
    {
        instance = this;
        _PlayerInputActions = new PlayerInputActions();
        _PlayerInputActions.Enable();

        _PlayerInputActions.combat.Attack.started += PlayerAttack_started;
        _PlayerInputActions.Player.Dash.performed += PlayerDash_performed;
    }

    private void PlayerDash_performed(InputAction.CallbackContext context)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
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
