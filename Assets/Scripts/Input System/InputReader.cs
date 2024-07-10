using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private PlayerControls inputs;

    public Vector2 MoveDirection { get; private set; }

    public bool Jumping { get; private set; }
    public bool Attacking { get; private set; }
    public bool Blocking { get; private set; }
    public bool Dashing { get; private set; }

    private void OnEnable()
    {
        if (inputs == null)
        {
            inputs = new PlayerControls();
            inputs.Enable();
            inputs.Player.SetCallbacks(this);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                MoveDirection = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                MoveDirection = Vector2.zero;
                break;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Jumping = true;
                break;
            case InputActionPhase.Canceled:
                Jumping = false;
                break;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Attacking = true;
                break;
            case InputActionPhase.Canceled:
                Attacking = false;
                break;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Dashing = true;
                break;
            case InputActionPhase.Canceled:
                Dashing = false;
                break;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Blocking = true;
                break;
            case InputActionPhase.Canceled:
                Blocking = false;
                break;
        }
    }
}
