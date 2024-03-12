using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input/OtherPlayerInput")]
public class OtherPlayerInput : ScriptableObject, OtherPlayerControls.IPlayerActions
{
    private OtherPlayerControls _otherPlayerControls;

    public Action<Vector2> OnMovement;
    public Action OnInteraction;
    public Action OnAttack;

    private void OnEnable()
    {
        if (_otherPlayerControls == null)
        {
            _otherPlayerControls = new OtherPlayerControls();
            _otherPlayerControls.Player.SetCallbacks(this);
        }
        _otherPlayerControls.Player.Enable();
    }

    void OtherPlayerControls.IPlayerActions.OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        OnMovement?.Invoke(inputVector);
    }

    void OtherPlayerControls.IPlayerActions.OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteraction?.Invoke();
        }
    }

    void OtherPlayerControls.IPlayerActions.OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnAttack?.Invoke();
        }
    }
}
