using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFXPrefab;

    public static Action<Health> OnDeath;

    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFXPrefab;
    [SerializeField] private int _startingHealth = 3;

    int _currentHealth;

    void Start() 
    {
        ResetHealth();
    }

    public void ResetHealth() 
    {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) 
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
