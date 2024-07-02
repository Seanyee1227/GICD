using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    Enemy Enemy;
    Player player;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1;
    public float moveSpeed = 10f;
    public float jumpPower = 1f;
    public float dashSpeed = 15f;
    public BoxCollider2D attackCollider;
    public int attackDamage = 10;
    public bool attacked;
    private int currentHP;
    private bool isGrounded = false;
    private bool isAttacking = false;
    private bool isDashing = false;
    private Rigidbody2D rb;
    private int _enemyLayer;
    

    private void Start()
    {
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        maxHp = 100;
        nowHp = 100;
        atkDmg = 10;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHp;

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D 컴포넌트가 없습니다.");
            enabled = false;
        }

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    private void Update()
    {
        Enemy = GetComponent<Enemy>();

        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
       
        if(Input.GetKey(KeyCode.Space)) 
        {
            verticalInput = 1f;
        }

        if (!isAttacking && !isDashing && Mathf.Abs(horizontalInput) > 0f)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

            if (horizontalInput > 0f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontalInput < 0f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpPower * moveSpeed, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (!isAttacking && !isDashing && Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(AttackCoroutine());
        }

        if (!isAttacking && !isDashing && Mathf.Abs(horizontalInput) > 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        yield return new WaitForSeconds(0.3f);

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }

        isAttacking = false;

        if (attackCollider.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;

        float dashDuration = 0.5f;
        float dashTimer = 0f;

        while (dashTimer < dashDuration)
        {
            rb.velocity = new Vector2(dashSpeed * dashDirection, rb.velocity.y);

            dashTimer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _enemyLayer)
        {
            Enemy.TakeDamage(atkDmg);
        }
    }

    private void OnCollision2D(Collision2D attackCollider)
    {
        if (attackCollider.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int atkDmg)
    {
        nowHp -= Enemy.atkDmg;
        if (currentHP <= 0)
        {
            Die();
        }
    }


    private void Die()
    {

    }
}