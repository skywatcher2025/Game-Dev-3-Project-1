using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public static Action OnExplode;
    public static Action OnFlash;
    
    [SerializeField] private Transform _aoeTransform;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] int _damageAmount = 5;
    [SerializeField] float _knockbackThrust = 50f;
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private float _radius = 3f;

    Vector2 _fireDirection;

    Rigidbody2D _rigidBody;
    private Flash _flash;
    private Gun _gun;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _flash = GetComponent<Flash>();
    }

    private void OnEnable()
    {
        OnExplode += Explode;
    }

    private void OnDisable()
    {
        OnExplode -= Explode;
    }

    public void Init(Vector2 grenadeSpawnPos, Vector2 mousePos, Gun gun)
    {
        _fireDirection = (mousePos - grenadeSpawnPos).normalized;
        float angle = Mathf.Atan2(_fireDirection.y, _fireDirection.x) * Mathf.Rad2Deg;
        transform.position = grenadeSpawnPos;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _rigidBody.velocity = _fireDirection * _moveSpeed;
        _gun = gun;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(.9f);
        _flash.StartFlash();
        OnFlash?.Invoke();
        yield return new WaitForSeconds(.9f);
        _flash.StartFlash();
        OnFlash?.Invoke();
        yield return new WaitForSeconds(.9f);
        _flash.StartFlash();
        OnFlash?.Invoke();
        yield return new WaitForSeconds(.3f);
        OnExplode?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            OnExplode?.Invoke();
        }
    }

    void Explode()
    {
        SpawnExplosionVFX();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, _radius);
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.GetComponent<Enemy>())
            {
                IHittable iHittable = col.gameObject.GetComponent<IHittable>();
                iHittable?.TakeHit();

                IDamageable iDamageable = col.gameObject.GetComponent<IDamageable>();
                iDamageable?.TakeDamage(_damageAmount,_knockbackThrust);
            
                //Debug.Log("Exploded Enemy!");
            }
        }
        Destroy(gameObject);
    }
    
    void SpawnExplosionVFX()
    {
        GameObject explosionVFX = Instantiate(_explosionVFX, gameObject.transform.position, gameObject.transform.rotation);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position,_radius);
    }
}
