using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpForce;
    private bool _isJump = false;

    [Header("PlayerAttack")]
    public GameObject attackRange;
    private KeyCode _attackKey = KeyCode.K;
    private bool _isAttacking = false;
    [SerializeField]
    private float _attackDelay = 0.3f;

    Rigidbody2D _rb;
    LayerMask _ground;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
       // attackRange = GetComponentInChildren<GameObject>();
    }

    private void Start()
    {

        _ground = LayerMask.NameToLayer("Ground");
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
        if (Input.GetButtonDown("Jump"))
        {
            if (!_isJump)
            {
                _isJump = true;
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator Attack()
    {
        if (Input.GetKey(_attackKey))
        {
            _isAttacking = true;
            attackRange.gameObject.SetActive(true);
            yield return new WaitForSeconds(_attackDelay);
            attackRange.gameObject.SetActive(false);
            _isAttacking = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _ground)
        {
            _isJump = false;
        }
    }
}
