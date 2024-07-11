using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Move")]
    public float moveSpeed;
    public float currentSpeed;
    public float jumpForce;
    private bool _isJumping;
    private bool _isdashing = false;
    private bool canDash = true;
    public float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCoolTime = 1f;

    [Header("PlayerAttack")]
    [SerializeField]
    private int _damage = 1;
    private float _curTime;
    private float _coolTime = 0.3f;
    public Transform pos;
    public Vector2 boxSize;
    public bool isAttacking = false;

    Rigidbody2D _rb;
    CapsuleCollider2D _coll;
    SpriteRenderer _sprite;
    public Animator anim;
    [SerializeField]
    TrailRenderer _tr;

    private void Awake()
    {
        instance = this;

        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<CapsuleCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
        _isJumping = false;
    }

    private void Update()
    {
        Jump();
        Attack();
        LandRayCast();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (_curTime > 0)
        {
            anim.SetBool("isAttacking", false);
        }
    }

    private void FixedUpdate()
    {
        if (_isdashing)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");

        if (!_isdashing)
        {
            _rb.velocity = new Vector2(hAxis * currentSpeed, _rb.velocity.y);
        }

        if (Input.GetButton("Horizontal"))
        {
            _sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if (Mathf.Abs(_rb.velocity.x) < 0.2f)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    private void Attack()
    {
        if (_curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.K) && isAttacking == false)
            { 
                isAttacking = true;

                Vector2 attackPos = pos.position;
                if (_sprite.flipX)
                {
                    attackPos.x -= boxSize.x;
                }

                Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos, boxSize, 0);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        collider.GetComponent<Enemy>().TakeDamage(_damage);
                    }
                }
                _curTime = _coolTime;
            }
        }
        else
        {
            _curTime -= Time.deltaTime;
            currentSpeed = moveSpeed;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            _isJumping = true;
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (_isdashing == true)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isDashing", true);
            }
            else
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isDashing", false) ;
            }
        }
    }

    private void LandRayCast()
    {
        if (_rb.velocity.y < 0)
        {
            Debug.DrawRay(_rb.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(_rb.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.8f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            _isJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 attackPos = pos.position;

        if (_sprite != null && _sprite.flipX)
        {
            attackPos.x -= boxSize.x;
        }
        Gizmos.DrawWireCube(attackPos, boxSize);
    }

    private IEnumerator Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            canDash = false;
            _isdashing = true;
            anim.SetBool("isDashing", true); 
            float originalGravity = _rb.gravityScale;
            _rb.gravityScale = 0f;

            float dashDirection = _sprite.flipX ? -1 : 1;
            _rb.velocity = new Vector2(dashDirection * dashForce, 0f);
            _tr.emitting = true;

            yield return new WaitForSeconds(dashTime);

            _rb.gravityScale = originalGravity;
            _tr.emitting = false;
            _isdashing = false;
            anim.SetBool("isDashing", false);  

            yield return new WaitForSeconds(dashCoolTime);
            canDash = true;
        }
    }
}
