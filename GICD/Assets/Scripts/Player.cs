using System;
using System.Collections;
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
    public GameObject attackRange;
    private KeyCode _attackKey = KeyCode.K;
    private bool _isAttacking = false;
    [SerializeField]
    private float _attackDelay = 0.3f;
    Transform _enemyPos;

    Rigidbody2D _rb;
    Collider2D _coll;
    BoxCollider2D _AttackRange;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _AttackRange = attackRange.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _isJump = false;
    }

    private void Update()
    {
        Jump();

        if (Input.GetKey(_attackKey) && _isAttacking == false)
        {
            StartCoroutine(Attack());
        }
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

    private IEnumerator Attack()
    {
        _isAttacking = true;
        AttackPos();
        attackRange.SetActive(true);
        yield return new WaitForSeconds(_attackDelay);
        attackRange.SetActive(false);
        _isAttacking = false;
    }

    private void AttackPos()
    {
        if (_lastMoveDir == Vector2.up)
        {
            _AttackRange.offset = new Vector2(0, 1);
            _AttackRange.size = new Vector2(1, 1);
        }
        else if (_lastMoveDir == Vector2.down)
        {
            _AttackRange.offset = new Vector2(0, -1);
            _AttackRange.size = new Vector2(1, 1);
        }
        else if (_lastMoveDir == Vector2.right)
        {
            _AttackRange.offset = new Vector2(1, 0);
            _AttackRange.size = new Vector2(1, 1);
        }
        else if (_lastMoveDir == Vector2.left)
        {
            _AttackRange.offset = new Vector2(-1, 0);
            _AttackRange.size = new Vector2(1, 1);
        }
    }
}
