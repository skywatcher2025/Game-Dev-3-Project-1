using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _jumpForce = 7f;
    [SerializeField] float _jumpInterval = 4f;
    [SerializeField] float _changeDirectionInterval = 3f;

    int _currentDirection;

    Rigidbody2D _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 newVelocity = new(_currentDirection * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = newVelocity;
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            _currentDirection = Random.Range(0, 2) * 2 - 1; // 1 or -1
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    IEnumerator RandomJump() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }
}
