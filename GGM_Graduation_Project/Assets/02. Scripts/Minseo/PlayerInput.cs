using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerControls;

    public Action<Vector2> OnMovement;
    public Action OnInteraction;    

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Player.Enable();
    }

    private void Update()
    {
        Vector2 inputVector = _playerControls.Player.Movement.ReadValue<Vector2>();
        OnMovement?.Invoke(inputVector);
        Debug.Log(inputVector);

        if (_playerControls.Player.Interaction.triggered)
        {
            OnInteraction?.Invoke();
        }
    }
}
