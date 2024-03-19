using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _jumpForce = 7f;
    [SerializeField] float _jumpInterval = 4f;
    [SerializeField] float _changeDirectionInterval = 3f;

    Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    void Start() 
    {
        Init();
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

    void Init()
    {
        _colorChanger.SetRandomColor();
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            float currentDirection = Random.Range(0, 2) * 2 - 1; // 1 or -1
            _movement.SetCurrentDirection(currentDirection);
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
