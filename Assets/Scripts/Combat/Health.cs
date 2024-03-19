using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _splatterPrefab;
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
            SpawnDeathSplatterPrefab();
            Destroy(gameObject);
        }
    }

    void SpawnDeathSplatterPrefab()
    {
        GameObject newSplatterPrefab = Instantiate(_splatterPrefab, transform.position, transform.rotation);
        SpriteRenderer deathSplatterRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = GetComponent<ColorChanger>();
        Color _currentColor = colorChanger.DefaultColor;
        deathSplatterRenderer.color = _currentColor;
    }
}
