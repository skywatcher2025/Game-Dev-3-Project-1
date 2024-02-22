using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int _startingHealth = 3;

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
            Destroy(gameObject);
        }
    }
}
