using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _maxHealth = 50f;
    [SerializeField]
    private float _currentHealth;

    Collider _collider;

    Player _player;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _collider = GetComponent<Collider>();
    }

    public void TakeDamage(int  _damage)
    {
        _currentHealth -= _damage;
        if (_currentHealth <= 0 )
        { 
            Destroy(gameObject);
        }
    }
}
