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

    [Header("PlayerAttack")]
    public GameObject attackRange;
    private KeyCode _attackKey = KeyCode.K;
    private bool _isAttacking = false;
    [SerializeField]
    private float _attackDelay = 0.3f;

    Rigidbody2D _rb;
    Collider2D _coll;

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
        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float _hAxis = Input.GetAxisRaw("Horizontal");

        _rb.velocity = new Vector2(_hAxis * _moveSpeed, _rb.velocity.y);
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
}
