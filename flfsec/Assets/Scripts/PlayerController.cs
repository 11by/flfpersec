using FMODUnity;
using System;
using System.Collections;
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
    public GameObject DeathBG;
    public GameObject DeathUI;
    public string jumpSound;
    public string landSound;

    bool isDie = false;
    int health = 3;

    private Rigidbody2D rb;
    private bool isGrounded; // ���� ���� �ִ��� ���� üũ
    private bool isJumping; // ���� ������ ���� üũ
    private float originalGravityScale; // ���� �߷� �� ����
    private Coroutine currentJumpRoutine; // ���� ���� ���� ���� �ڷ�ƾ
    private Pause pause; // Pause ��ũ��Ʈ ����
    private float timeSlowFactor = 0.05f; // �ð� ���� ����
    private MusicController musiccontroller;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // ���� �� �߷� �� ����
        CheckGrounded();

        health = maxHealth;
        dustParticle.Stop();
        pause = FindObjectOfType<Pause>(); // Pause ��ũ��Ʈ ����
        DeathUI.SetActive(false);
    }

    void Update()
    {
        // ���� �Է� üũ
        if (Input.GetKeyDown(KeyCode.Space) && (pause == null || !pause.IsPause))
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
        animator.Play("Die");

        // �÷��� ��ũ�� ������ ���߱�
        StartCoroutine(SlowDownPlatformScrolling());

        StartCoroutine(SlowDownTimeAndShowDeathUI());
        if (musiccontroller != null)
        {
            musiccontroller.StopMusic();
        }
    }

    IEnumerator SlowDownPlatformScrolling()
    {
        PlatformScroller[] platformScrollers = FindObjectsOfType<PlatformScroller>();
        float elapsedTime = 0f;
        float duration = 1f; // ������ ���ߴ� �� �ɸ��� �ð�

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            foreach (PlatformScroller scroller in platformScrollers)
            {
                scroller.scrollSpeed = Mathf.Lerp(scroller.scrollSpeed, 0f, t);
            }

            yield return null;
        }

        // ���������� �ӵ��� 0���� ����
        foreach (PlatformScroller scroller in platformScrollers)
        {
            scroller.scrollSpeed = 0f;
        }
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
        PlayjumpSound();
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
            PlaylandSound();
            animator.Play("Land");
        }
        else
        {
            rb.gravityScale = 100;
            isGrounded = false;

            Die();
        }

        isJumping = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Dead"))
        {
            rb.velocity = Vector2.zero;
            Die();
        }
    }

    IEnumerator SlowDownTimeAndShowDeathUI()
    {
        float elapsedTime = 0f;
        float duration = 1f; // 1�� ���� �ð� ������

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Time.timeScale = Mathf.Lerp(1f, timeSlowFactor, t); // �ð� ���� ���̱�
            yield return null; // �� ������ ��ٸ� (Time.unscaledDeltaTime ���)
        }

        Time.timeScale = 0; // ������ ����
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

    void PlayjumpSound()
    {
        if (!string.IsNullOrEmpty(jumpSound))
        {
            RuntimeManager.PlayOneShot(jumpSound);
        }
    }

    void PlaylandSound()
    {
        if (!string.IsNullOrEmpty(landSound))
        {
            RuntimeManager.PlayOneShot(landSound);
        }
    }
}
