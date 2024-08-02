using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 5f; // 점프 높이
    public float airTime = 1f; // 공중에 떠 있는 시간
    public LayerMask groundLayer; // 발판 레이어 마스크
    public float groundCheckDistance = 1.0f; // 발판 검사 거리
    public float fixedXPosition = -5.5f; // 플레이어의 고정된 x 좌표
    public int maxHealth = 3; // 플레이어의 목숨
    public Animator animator;
    public ParticleSystem dustParticle;
    public GameObject DeathUI;

    bool isDie = false;
    int health = 3;

    private Rigidbody2D rb;
    private bool isGrounded; // 발판 위에 있는지 여부 체크
    private bool isJumping; // 점프 중인지 여부 체크
    private float originalGravityScale; // 원래 중력 값 저장
    private Coroutine currentJumpRoutine; // 현재 진행 중인 점프 코루틴

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // 시작 시 중력 값 저장
        CheckGrounded();

        health = maxHealth;
        dustParticle.Stop();
    }

    void Update()
    {
        // 점프 입력 체크
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
                // 플레이어가 공중 상태이면서 아래 방향에 발판이 없는 경우
                return;
            }
        }

        // 애니메이션 설정
        animator.SetBool("IsAirborne", !isGrounded);

        // 파티클 이펙트 제어
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

        // 플레이어의 x 좌표 고정
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
        // 발판 위에 있는지 여부 체크하는 레이캐스트
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

        // 점프 시작
        rb.gravityScale = 0; // 중력 값 0으로 설정
        Vector3 startPosition = transform.position;
        Vector3 peakPosition = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);

        // 점프 높이로 순간 이동
        transform.position = peakPosition;

        // 공중에 떠 있는 시간 동안 대기
        yield return new WaitForSeconds(airTime);

        isJumping = false;
        // 착지 확인 및 상태 업데이트
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

    // 플레이어 아래에 발판이 있는지 확인
    bool IsPlatformBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        return hit.collider != null && hit.collider.CompareTag("Platform");
    }
}
