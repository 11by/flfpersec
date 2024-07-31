using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 5f; // 점프 높이
    public float airTime = 1f; // 공중에 떠 있는 시간
    public LayerMask groundLayer; // 발판 레이어 마스크
    public float groundCheckDistance = 1.0f; // 발판 검사 거리
    public float fixedXPosition = -5.5f; // 플레이어의 고정된 x 좌표
    public int lives = 3; // 플레이어의 목숨
    public Animator animator;
    public ParticleSystem dustParticle;

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
    }

    void Update()
    {
        // 점프 입력 체크
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && !isJumping)
            {
                currentJumpRoutine = StartCoroutine(NormalJump(jumpHeight));
            }
            else if (isJumping)
            {
                LandOnClosestPlatformBelow();
            }
        }

        // 애니메이션 설정
        animator.SetBool("IsAirborne", !isGrounded);

        // 파티클 이펙트 제어
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

        // 플레이어의 x 좌표 고정
        Vector3 position = transform.position;
        position.x = fixedXPosition;
        transform.position = position;
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

        // 공중에 떠 있는 동안 대기
        float airTime = 1.0f; // 공중에 떠 있는 최대 시간
        float timeInAir = 0f;

        while (timeInAir < airTime)
        {
            // 점프 중 입력이 있는지 확인
            if (Input.GetButton("Jump")) // "Jump" 버튼이 눌린 경우
            {
                // 입력이 있으므로 계속 공중에 머물러 있음
                timeInAir = 0; // 시간을 초기화
            }
            else
            {
                timeInAir += Time.deltaTime; // 입력이 없으면 공중에 있는 시간 증가
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 입력이 없어서 1초가 지나면 착지
        LandOnClosestPlatformBelow();

        // 땅으로 떨어지는 애니메이션 추가 (필요시)
        // animator.SetTrigger("Fall");

        // 점프 루틴 종료
        isJumping = false;
    }

    IEnumerator NormalJump(float height)
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

        // 땅으로 순간 이동
        LandOnClosestPlatformBelow();
        CheckGrounded();

        // 착지할 때의 애니메이션 트리거
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
