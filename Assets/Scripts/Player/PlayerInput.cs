using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    private PlayerInputActions _playerInputActions;
    private InputAction _move;
    private InputAction _jump;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _move = _playerInputActions.Player.Move;
        _jump = _playerInputActions.Player.Jump;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    void Update()
    {
        FrameInput = GatherInput();
    }
    
    FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame(),
        };
    }
}

//Structs are not the same between C++ and C#
public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
}