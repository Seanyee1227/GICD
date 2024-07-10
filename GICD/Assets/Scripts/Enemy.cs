using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private float _Health = 50f;

    Collider _collider;

    Player _player;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void TakeDamage(int  _damage)
    {
        _Health -= _damage;
        if (_Health <= 0 )
        { 
            Destroy(gameObject);
        }
    }
}
