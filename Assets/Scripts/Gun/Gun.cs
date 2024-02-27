using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] Bullet _bulletPrefab;

    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Shoot();
        RotateGun();
    }

    void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += FireAnimation;
    }

    void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= FireAnimation;
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            OnShoot?.Invoke(); //Question mark (?) checks if "OnShoot" is null
        }
    }

    void ShootProjectile()
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, _mousePos);
    }

    void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0,0f);
    }

    void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0,0,angle);
        
    }
}
