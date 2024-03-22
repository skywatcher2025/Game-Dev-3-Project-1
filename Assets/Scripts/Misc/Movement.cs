using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private float _moveX;
    private bool _canMove = true;

    private Rigidbody2D _rigidbody;
    private Knockback _knockback;
    //private PlayerController _playerController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
        //_playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        _knockback.OnKnockbackStart += CanMoveFalse;    //Creates Listener
        _knockback.OnKnockbackEnd += CanMoveTrue;       //Creates Listener
        
        /*
        if (_playerController)
        {
            _playerController.OnDashStart += CanMoveFalse;
            _playerController.OnDashEnd += CanMoveTrue;
        }
        */
    }

    private void OnDisable()
    {
        _knockback.OnKnockbackStart -= CanMoveFalse;    //Removes Listener
        _knockback.OnKnockbackEnd -= CanMoveTrue;       //Removes Listener
        
        /*
        if (_playerController)
        {
            _playerController.OnDashStart -= CanMoveFalse;
            _playerController.OnDashEnd -= CanMoveTrue;
        }
        */
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void CanMoveTrue() //change from public later
    {
        _canMove = true;
    }

    public void CanMoveFalse() //change from public later
    {
        _canMove = false;
    }

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    void Move()
    {
        if (!_canMove) return;
        
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = movement;
    }
}
