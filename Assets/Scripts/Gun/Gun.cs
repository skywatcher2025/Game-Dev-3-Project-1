using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] Bullet _bulletPrefab;

    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;

    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;
    private ObjectPool<Bullet> _bulletPool;

    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        CreateBulletPool();
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
        OnShoot += ScreenShake;
    }

    void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= FireAnimation;
        OnShoot -= ScreenShake;
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0)) 
        {
            OnShoot?.Invoke(); //Question mark (?) checks if "OnShoot" is null
        }
    }

    void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        //Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, _mousePos, this);
    }

    void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0,0f);
    }

    void ScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0,0,angle);
        
    }
    
    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet); //happens at some point
    }

    void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bulletPrefab); //Instantiate whatever prefab you're using

        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
        }, bullet1 =>
        {
            bullet1.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet);
        }, false, 10, 20);
    }
}
