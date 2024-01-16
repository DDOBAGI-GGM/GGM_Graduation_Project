using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input/PlayerInput")]
public class PlayerInput : ScriptableObject, PlayerControls.IPlayerActions
{
    private PlayerControls _playerControls;

    public Action<Vector2> OnMovement;
    public Action OnInteraction;    

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.SetCallbacks(this);
        }
        _playerControls.Player.Enable();
    }

    void PlayerControls.IPlayerActions.OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        OnMovement?.Invoke(inputVector);
    }

    void PlayerControls.IPlayerActions.OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteraction?.Invoke();
        }
    }
}
