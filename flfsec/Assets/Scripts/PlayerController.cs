using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 5f; // ���� ����
    public float airTime = 1f; // ���߿� �� �ִ� �ð�
    public LayerMask groundLayer; // ���� ���̾� ����ũ
    public float groundCheckDistance = 1.0f; // ���� �˻� �Ÿ�
    public float fixedXPosition = -5.5f; // �÷��̾��� ������ x ��ǥ
    public int maxHealth = 3; // �÷��̾��� ���
    public Animator animator;
    public ParticleSystem dustParticle;
    public GameObject DeathUI;

    bool isDie = false;
    int health = 3;

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

        health = maxHealth;
        dustParticle.Stop();
    }

    void Update()
    {
        // ���� �Է� üũ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && !isJumping)
            {
                currentJumpRoutine = StartCoroutine(JumpRoutine(jumpHeight));
            }
            else if (isJumping && IsPlatformBelow())
            {
                LandOnClosestPlatformBelow();
            }
            else if (isJumping && !IsPlatformBelow())
            {
                // �÷��̾ ���� �����̸鼭 �Ʒ� ���⿡ ������ ���� ���
                return;
            }
        }

        // �ִϸ��̼� ����
        animator.SetBool("IsAirborne", !isGrounded);

        // ��ƼŬ ����Ʈ ����
        if (isGrounded)
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

        if (health == 0)
        {
            if (!isDie)
            {
                Die();
            }
            return;
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();

        // �÷��̾��� x ��ǥ ����
        Vector3 position = transform.position;
        position.x = fixedXPosition;
        transform.position = position;

        if (health == 0)
        {
            return;
        }
    }

    void Die()
    {
        isDie = true;

        rb.velocity = Vector2.zero;
        animator.Play("Die");
        StartCoroutine(ShowDeathUI());
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
        yield return new WaitForSeconds(airTime);

        isJumping = false;
        // ���� Ȯ�� �� ���� ������Ʈ
        LandOnClosestPlatformBelow();
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
            rb.gravityScale = originalGravityScale;
            animator.Play("Land");
        }
        else
        {
            isGrounded = false;
        }

        isJumping = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Dead"))
        {
            Die();
            Time.timeScale = 0f;
        }
    }

    IEnumerator ShowDeathUI()
    {
        yield return new WaitForSecondsRealtime(1f);
        DeathUI.SetActive(true);
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

        if (transform.position.y < 0.3)
        {
            LandOnClosestPlatformBelow();
        }
        else
        {
            currentJumpRoutine = StartCoroutine(JumpRoutine(-jumpHeight));
        }
    }

    // �÷��̾� �Ʒ��� ������ �ִ��� Ȯ��
    bool IsPlatformBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        return hit.collider != null && hit.collider.CompareTag("Platform");
    }
}
