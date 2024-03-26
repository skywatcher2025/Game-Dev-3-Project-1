using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] int _damageAmount = 1;
    [SerializeField] float _knockbackThrust = 20f;
    [SerializeField] private GameObject _shatterVFX;

    Vector2 _fireDirection;

    Rigidbody2D _rigidBody;
    private Gun _gun;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnDestroy()
    {
        SpawnShatterVFX();
    }

    private void OnDisable()
    {
        SpawnShatterVFX();
    }

    public void Init(Vector2 bulletSpawnPos, Vector2 mousePos, Gun gun)
    {
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
        float angle = Mathf.Atan2(_fireDirection.y, _fireDirection.x) * Mathf.Rad2Deg;
        transform.position = bulletSpawnPos;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _gun = gun;
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        //Health health = other.gameObject.GetComponent<Health>();
        //health?.TakeDamage(_damageAmount);

        //Knockback knockback = other.gameObject.GetComponent<Knockback>();
        //knockback?.GetKnockedBack(PlayerController.Instance.transform.position, _knockbackThrust);

        //Flash flash = other.gameObject.GetComponent<Flash>();
        //flash?.StartFlash();

        IHittable iHittable = other.gameObject.GetComponent<IHittable>();
        iHittable?.TakeHit();

        IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
        iDamageable?.TakeDamage(_damageAmount,_knockbackThrust);
        
        _gun.ReleaseBulletFromPool(this);
        //Destroy(gameObject);
    }
    
    void SpawnShatterVFX()
    {
        GameObject shatterVFX = Instantiate(_shatterVFX, this.transform.position, this.transform.rotation);
        shatterVFX.transform.SetParent(this.transform);
    }
}