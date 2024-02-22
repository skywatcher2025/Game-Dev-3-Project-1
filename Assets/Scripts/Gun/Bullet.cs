using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] int _damageAmount = 1;

    Vector2 _fireDirection;

    Rigidbody2D _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        if (PlayerController.Instance.IsFacingRight()) 
        {
            _fireDirection = Vector2.right;
        } else 
        {
            _fireDirection = Vector2.left;
        }
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        Destroy(gameObject);
    }
}