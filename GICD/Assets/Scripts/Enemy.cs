using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private float _Health = 5f;
    private float moveSpeed = 5f;
    public float currentSpeed;

    [Header("Attack")]
    public int damage = 1;

    Collider _collider;

    Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
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
