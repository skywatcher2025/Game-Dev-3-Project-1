using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnJump;
    //public Action OnDashStart;
    //public Action OnDashEnd;
    public static PlayerController Instance;

    [SerializeField] Transform _footTransform;
    [SerializeField] Vector2 _groundCheck;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _jumpStrength = 7f;
    [SerializeField] float _dashStrength = 7f;
    [SerializeField] float _extraGravity = 900f;
    [SerializeField] float _maxFallSpeed = -25f;
    [SerializeField] float _gravityDelay = .2f;
    [SerializeField] float _coyoteTime = 1f;
    [SerializeField] private float _dashDelay = .5f;
    [SerializeField] float  _dashTime = .2f;
    [SerializeField] private GameObject _poofVFX;

    private float _timeInAir, _coyoteTimer;
    private bool _doubleJumpAvailable;
    private float  _dashTimer;
    private bool _dashAvailable;
    
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

        _dashAvailable = true;
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
        DashTimer();
        HandleDash();
        HandleSpriteFlip();
        GravityDelay();
        
        //CustomDebug();
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

            Instantiate(_poofVFX, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), transform.rotation);
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
    
    //####################################

    void HandleDash()
    {
        if(!_frameInput.DashLeft && !_frameInput.DashRight) return;

        if (_frameInput.DashLeft && _dashAvailable)
        {
            //OnDashStart?.Invoke();
            _movement.CanMoveFalse();
            ApplyDashForce(true);
            _dashAvailable = false;
            _dashTimer = _dashDelay;
        }

        if (_frameInput.DashRight && _dashAvailable)
        {
            //OnDashStart.Invoke();
            _movement.CanMoveFalse();
            ApplyDashForce(false);
            _dashAvailable = false;
            _dashTimer = _dashDelay;
        }
    }
    
    public void ApplyDashForce(bool left)
    {
        //_rigidBody.velocity = Vector2.zero;
        
        if (left)
        {
            _rigidBody.AddForce(Vector2.left * _dashStrength, ForceMode2D.Impulse);
            StartCoroutine(DashRoutine());
        }
        else
        {
            _rigidBody.AddForce(Vector2.right * _dashStrength, ForceMode2D.Impulse);
            StartCoroutine(DashRoutine());
        }
    }

    void DashTimer()
    {
        if(_dashTimer < 0)
        {
            _dashTimer = 0;
        }

        if (_dashTimer == 0)
        {
            _dashAvailable = true;
        }
        else
        {
            _dashTimer -= Time.deltaTime;
        }
    }
    
    IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(_dashTime);
        StopDashRoutine();
        //OnDashEnd?.Invoke();
        _movement.CanMoveTrue();
    }

    void StopDashRoutine()
    {
        _rigidBody.velocity = Vector2.zero;
    }
    
    //####################################

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

    void CustomDebug()
    {
        if(_frameInput.Jump) Debug.Log("Space pressed");
        if(_frameInput.DashLeft) Debug.Log("Q pressed");
        if(_frameInput.DashRight) Debug.Log("E pressed");
    }
}
