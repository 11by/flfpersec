using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 5f; // ���� ����
    public float airTime = 1f; // ���߿� �� �ִ� �ð�
    public LayerMask groundLayer; // ���� ���̾� ����ũ
    public float groundCheckDistance = 1.0f; // ���� �˻� �Ÿ�
    public float fixedXPosition = -5.5f; // �÷��̾��� ������ x ��ǥ
    public int lives = 3; // �÷��̾��� ���
    public Animator animator;
    public ParticleSystem dustParticle;

    private Rigidbody2D rb;
    private bool isGrounded; // ���� ���� �ִ��� ���� üũ
    private bool isJumping; // ���� ������ ���� üũ
    private float originalGravityScale; // ���� �߷� �� ����
    private Coroutine currentJumpRoutine; // ���� ���� ���� ���� �ڷ�ƾ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // ���� �� �߷� �� ����
        CheckGrounded();
    }

    void Update()
    {
        // ���� �Է� üũ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            currentJumpRoutine = StartCoroutine(NormalJump(jumpHeight));
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isJumping)
        {
            LandOnClosestPlatformBelow();
        }

        // �ִϸ��̼� ����
        animator.SetBool("IsAirborne", !isGrounded);

        // ��ƼŬ ����Ʈ ����
        if (isGrounded && !isJumping)
        {
            if (!dustParticle.isPlaying)
            {
                dustParticle.Play();
            }
        }
        else
        {
            if (dustParticle.isPlaying)
            {
                dustParticle.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();

        // �÷��̾��� x ��ǥ ����
        Vector3 position = transform.position;
        position.x = fixedXPosition;
        transform.position = position;
    }

    void CheckGrounded()
    {
        // ���� ���� �ִ��� ���� üũ�ϴ� ����ĳ��Ʈ
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null && hit.collider.CompareTag("Platform"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    IEnumerator JumpRoutine(float height)
    {
        isJumping = true;
        isGrounded = false;
        animator.SetTrigger("Jump");

        // ���� ����
        rb.gravityScale = 0; // �߷� �� 0���� ����
        Vector3 startPosition = transform.position;
        Vector3 peakPosition = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);

        // ���� ���̷� ���� �̵�
        transform.position = peakPosition;

        // ���߿� �� �ִ� �ð� ���� ���
        yield return new WaitForSeconds(0);

        // ���� ��ƾ ����
        isJumping = false;
    }

    IEnumerator NormalJump(float height)
    {
        isJumping = true;
        isGrounded = false;
        animator.SetTrigger("Jump");

        // ���� ����
        rb.gravityScale = 0; // �߷� �� 0���� ����
        Vector3 startPosition = transform.position;
        Vector3 peakPosition = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);

        // ���� ���̷� ���� �̵�
        transform.position = peakPosition;

        // ���߿� �� �ִ� �ð� ���� ���
        yield return new WaitForSeconds(airTime);

        // ������ ���� �̵�
        Vector3 groundPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        transform.position = groundPosition;

        // �߷� �� ������� ����
        rb.gravityScale = originalGravityScale;
        isJumping = false;
        CheckGrounded();

        // ������ ���� �ִϸ��̼� Ʈ����
        if (isGrounded)
        {
            animator.SetTrigger("Land");
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
            Debug.Log("Player hit by enemy! Lives remaining: " + lives);
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
    }

    public void JumpUp()
    {
        if (currentJumpRoutine != null)
        {
            StopCoroutine(currentJumpRoutine);
        }
        currentJumpRoutine = StartCoroutine(JumpRoutine(jumpHeight));
    }

    public void JumpDown()
    {
        if (currentJumpRoutine != null)
        {
            StopCoroutine(currentJumpRoutine);
        }
        currentJumpRoutine = StartCoroutine(JumpRoutine(-jumpHeight));
    }

    void LandOnClosestPlatformBelow()
    {
        if (currentJumpRoutine != null)
        {
            StopCoroutine(currentJumpRoutine);
            currentJumpRoutine = null;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        if (hit.collider != null)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            isGrounded = true;
            isJumping = false;
            rb.gravityScale = originalGravityScale;
            animator.SetTrigger("Land");
        }
    }
}