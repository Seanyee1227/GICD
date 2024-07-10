using TMPro;
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
    CapsuleCollider2D _coll;
    SpriteRenderer _sprite;
    Animator _anim;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<CapsuleCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _isJump = false;
    }

    private void Update()
    {
        Jump();
        Attack();
        LandRayCast();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float _hAxis = Input.GetAxisRaw("Horizontal");

        _rb.velocity = new Vector2(_hAxis * _moveSpeed, _rb.velocity.y);

       if (Input.GetButton("Horizontal"))
        {
            _sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

       if (Mathf.Abs(_rb.velocity.x) < 0.2f)
        {
            _anim.SetBool("isRunning", false);
        }
        else
        {
            _anim.SetBool("isRunning", true);
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
            _anim.SetBool("isJumping", true);
            _anim.SetBool("isRunning", false);
        }
    }

    private void LandRayCast()
    {
        if (_rb.velocity.y < 0)
        {
            // Ray 그리기
            Debug.DrawRay(_rb.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D _rayHit = Physics2D.Raycast(_rb.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (_rayHit.collider != null)
            {
                if (_rayHit.distance < 0.8f)
                {
                    _anim.SetBool("isJumping", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJump = false;
        }
    }

    // 공격 범위
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
