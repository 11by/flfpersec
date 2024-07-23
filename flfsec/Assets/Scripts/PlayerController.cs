using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // ���� ��
    public float downwardForce = -50f; // ���߿��� ���� �� �Ʒ��� �������� ��
    public LayerMask groundLayer; // ���� ���̾� ����ũ
    public float groundCheckDistance = 1.0f; // ���� �˻� �Ÿ�
    public float fixedXPosition = -5.5f; // �÷��̾��� ������ x ��ǥ
    public int lives = 3; // �÷��̾��� ���
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded; // ���� ���� �ִ��� ���� üũ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CheckGrounded();
    }

    void Update()
    {
        // ���� �Է� üũ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                // ���߿��� ���� Ű�� ������ �� �Ʒ��� ���� ���� ���� �����ϵ��� ó��
                ApplyDownwardForce();
            }
        }

        if (!isGrounded)
        {
            animator.SetBool("IsAirborne", true);
        }
        else
        {
            animator.SetBool("IsAirborne", false);
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();

        Vector3 position = transform.position;
        position.x = fixedXPosition;
        transform.position = position;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // ���� �Ŀ��� ���߿� �ִ� ���·� ����
        animator.SetTrigger("Jump");
    }

    void ApplyDownwardForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, downwardForce);
    }

    void CheckGrounded()
    {
        // ���� ���� �ִ��� ���� üũ�ϴ� ����ĳ��Ʈ
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null && hit.collider.CompareTag("Platform"))
        {
            if (Mathf.Abs(hit.distance) < 0.1f)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();
        }
        else if (collision.gameObject.CompareTag("Dead"))
        {
            Die();
        }
    }

    void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            Die();
        }
        else
        {
            // �÷��̾ ������ �¾��� ���� ���� �߰� (��: ������, ���� �ð� ��)
            Debug.Log("Player hit by enemy! Lives remaining: " + lives);
        }
    }

    void Die()
    {
        // �÷��̾ ������� ���� ���� �߰�
        Debug.Log("Player has died!");
        // ���� ���, ���� ���� ȭ������ ��ȯ�ϰų� �÷��̾ �����ϴ� �ڵ带 �߰��� �� �ֽ��ϴ�.
    }

    public void CheckCollisionWithTag(string tag)
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        if (obj != null)
        {
            if (tag == "Enemy")
            {
                LoseLife();
            }
            else if (tag == "Dead")
            {
                Die();
            }
        }
    }
}