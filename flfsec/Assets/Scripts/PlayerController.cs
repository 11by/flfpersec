using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // 점프 힘
    public float downwardForce = -50f; // 공중에서 점프 시 아래로 가해지는 힘
    public LayerMask groundLayer; // 발판 레이어 마스크
    public float groundCheckDistance = 1.0f; // 발판 검사 거리
    public float fixedXPosition = -5.5f; // 플레이어의 고정된 x 좌표
    public int lives = 3; // 플레이어의 목숨
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded; // 발판 위에 있는지 여부 체크

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CheckGrounded();
    }

    void Update()
    {
        // 점프 입력 체크
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                // 공중에서 점프 키를 눌렀을 때 아래로 강한 힘을 가해 착지하도록 처리
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
        isGrounded = false; // 점프 후에는 공중에 있는 상태로 변경
        animator.SetTrigger("Jump");
    }

    void ApplyDownwardForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, downwardForce);
    }

    void CheckGrounded()
    {
        // 발판 위에 있는지 여부 체크하는 레이캐스트
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
            // 플레이어가 적에게 맞았을 때의 로직 추가 (예: 깜빡임, 무적 시간 등)
            Debug.Log("Player hit by enemy! Lives remaining: " + lives);
        }
    }

    void Die()
    {
        // 플레이어가 사망했을 때의 로직 추가
        Debug.Log("Player has died!");
        // 예를 들어, 게임 오버 화면으로 전환하거나 플레이어를 리셋하는 코드를 추가할 수 있습니다.
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