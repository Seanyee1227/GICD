using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpForce;
    private bool _isJump;
    private Vector2 _lastMoveDir = Vector2.down;

    [Header("PlayerAttack")]
    private int _damage = 10;
    private float _curTime;
    private float _coolTime;
    public Transform pos;
    public Vector2 boxSize;

    Rigidbody2D _rb;
    Collider2D _coll;
    BoxCollider2D _AttackRange;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _isJump = false;
    }

    private void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float _hAxis = Input.GetAxisRaw("Horizontal");
        float _vAxis = Input.GetAxisRaw("Vertical");

        _rb.velocity = new Vector2(_hAxis * _moveSpeed, _rb.velocity.y);
        Vector3 _MoveDir = new Vector3(_hAxis, _vAxis, 0).normalized;

        if (_MoveDir != Vector3.zero)
        {
            _lastMoveDir = _MoveDir;
        }
    }

    private void  Attack()
    {
        if (_curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Collider2D[] _coll2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in _coll2D)
                {
                    if (collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().TakeDamage(10);
                    }   
                }
                _curTime = _coolTime;
            }
        }
        else
        {
            _curTime -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _isJump = true;
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJump = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
