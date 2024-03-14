using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnJump;
    public static PlayerController Instance;

    [SerializeField] Transform _footTransform;
    [SerializeField] Vector2 _groundCheck;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _jumpStrength = 7f;
    [SerializeField] float _extraGravity = 900f;
    [SerializeField] float _maxFallSpeed = -25f;
    [SerializeField] float _gravityDelay = .2f;
    [SerializeField] float _coyoteTime = 1f;

    private float _timeInAir, _coyoteTimer;
    private bool _doubleJumpAvailable;
    
    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    Rigidbody2D _rigidBody;
    Animator _animator;
    private Movement _movement;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
    }

    void Update()
    {
        GatherInput();
        Move();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
    }

    void FixedUpdate()
    {
        ExtraGravity();
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_footTransform.position, _groundCheck, 0f, _groundLayer);
        return isGrounded;
    }

    void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    void Move() 
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
        
        if(Mathf.Abs(_rigidBody.velocity.x) < Mathf.Epsilon)
            _animator.SetBool("Running", false);
        else
            _animator.SetBool("Running", true); 
        
        if (_rigidBody.velocity.y < 0)
        {
            _animator.SetBool("Running", false);
            _animator.SetBool("Falling", true);
        }
        else if (_rigidBody.velocity.y >= 0)
        {
            _animator.SetBool("Falling", false);
        }
    }

    void HandleJump()
    {
        if(!_frameInput.Jump) return;

        if (CheckGrounded())
        {
            OnJump?.Invoke();               // '?' checks if 'OnJump' is null first
        }
        else if (_coyoteTimer > 0)
        {
            OnJump?.Invoke();               // '?' checks if 'OnJump' is null first
        }
        else if (_doubleJumpAvailable)
        {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();               // '?' checks if 'OnJump' is null first
        }
    }

    void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvailable = true;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }
    
    void ApplyJumpForce()
    {
        _rigidBody.velocity = Vector2.zero;
        _timeInAir = 0;
        _coyoteTimer = 0;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void GravityDelay()
    {
        if (!CheckGrounded())
        {
            _timeInAir += Time.deltaTime;
        }
        else
        {
            _timeInAir = 0;
        }
    }

    void ExtraGravity()
    {
        if (_timeInAir >= _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
            if (_rigidBody.velocity.y < _maxFallSpeed)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _maxFallSpeed);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_footTransform.position,_groundCheck);
    }
}
