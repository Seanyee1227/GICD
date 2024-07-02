using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp = 50;
    public int nowHp;
    public int atkDmg;
    public int atkSpeed;

    public float detectionRange = 8f;
    public float moveSpeed = 5f;
    private int _playerLayer;

    private Transform playerTransform;
    private Player player;
    public Collider2D attackCollider;


    void Start()
    {
        nowHp = maxHp;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        attackCollider = GetComponentInChildren<Collider2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _playerLayer = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= detectionRange)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void SetEnemyStatus(int _maxHp, int _atkDmg, int _atkSpeed)
    {
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col);

        if (col.gameObject.layer == _playerLayer)
        {
            Debug.Log("Get Attack!!");
            Player player = col.transform.GetComponent<Player>();
            if (player != null)
            {
                if(player.attacked)
                {
                    TakeDamage(atkDmg);
                }
                player.TakeDamage(atkDmg);
                nowHp -= player.atkDmg;
                Debug.Log("Enemy nowHp: " + nowHp);
                if (nowHp <= 0)
                {
                    Die();
                }
            }
        }
    }

    public void TakeDamage(int atkDmg) 
    {
       
        nowHp -= player.atkDmg;
        if (nowHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
