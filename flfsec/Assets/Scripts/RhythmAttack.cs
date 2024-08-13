using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public Vector2 attackOffset; // 공격 범위의 위치 오프셋
    public LayerMask enemyLayer;
    public Animator animator; // Animator 추가
    public PlayerController playerController; // PlayerController 추가

    [Header("Particle Effects")]
    public GameObject perfectParticlePrefab; // Perfect 판정 파티클 프리팹
    public GameObject greatParticlePrefab;   // Great 판정 파티클 프리팹
    public GameObject goodParticlePrefab;    // Good 판정 파티클 프리팹
    public GameObject poorParticlePrefab;    // Poor 판정 파티클 프리팹

    public string hitSoundEventPath;
    public string swingSoundEventPath;
    private Pause pause; // Pause 스크립트 참조 변수
    private ScoreManager scoreManager; // ScoreManager 참조 변수

    [Header("Judgement Ranges")]
    float perfectRange = 0.5f;
    float greatRange = 1.0f;
    float goodRange = 1.5f;
    float poorRange = 2.0f;

    [Header("Judgement Offset")]
    public Vector2 judgementOffset = Vector2.zero; // 판정 범위의 중심 오프셋

    void Start()
    {
        pause = FindObjectOfType<Pause>(); // Pause 스크립트 참조
        scoreManager = FindObjectOfType<ScoreManager>(); // ScoreManager 스크립트 참조
    }

    void Update()
    {
        // 일시정지 상태가 아닌 경우에만 공격 입력 처리
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L)) && (pause == null || !pause.IsPause))
        {
            Attack();
        }
    }

    void Attack()
    {
        bool isAirborne = animator.GetBool("IsAirborne");
        PlaySwingSound();

        // 애니메이션 트리거 설정
        if (isAirborne)
        {
            animator.SetTrigger("AttackTrigger");
        }
        else
        {
            int randomAttack = Random.Range(1, 3);
            if (randomAttack == 1)
            {
                animator.SetTrigger("AttackTrigger");
            }
            else
            {
                animator.SetTrigger("AttackTrigger2");
            }
        }

        // 공격 범위 내의 적을 감지
        Vector2 attackPosition = (Vector2)transform.position + attackOffset + judgementOffset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayer);

        // 가장 왼쪽에 있는 적을 찾음
        Collider2D leftmostHit = null;
        float leftmostX = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.CompareTag("Enemy"))
            {
                if (hit.transform.position.x < leftmostX)
                {
                    leftmostX = hit.transform.position.x;
                    leftmostHit = hit;
                }
            }
        }

        // 가장 왼쪽에 있는 적 처리
        if (leftmostHit != null)
        {
            GameObject enemy = leftmostHit.gameObject;
            float distanceToPlayer = Vector2.Distance(leftmostHit.transform.position, transform.position);

            // 적의 이름에 따른 플레이어의 동작 설정
            if (enemy.name.Contains("Up"))
            {
                playerController.JumpUp();
            }
            else if (enemy.name.Contains("Down"))
            {
                playerController.JumpDown();
            }

            // 판정에 따른 점수 처리 및 파티클 이펙트 재생
            distanceToPlayer -= 1.4f;
            if (distanceToPlayer <= perfectRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Perfect);
                Instantiate(perfectParticlePrefab, enemy.transform.position, Quaternion.identity); // Perfect 파티클
                Debug.Log("Perfect");
            }
            else if (distanceToPlayer <= greatRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Great);
                Instantiate(greatParticlePrefab, enemy.transform.position, Quaternion.identity); // Great 파티클
                Debug.Log("Great");
            }
            else if (distanceToPlayer <= goodRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Good);
                Instantiate(goodParticlePrefab, enemy.transform.position, Quaternion.identity); // Good 파티클
                Debug.Log("Good");
            }
            else if (distanceToPlayer <= poorRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Poor);
                Instantiate(poorParticlePrefab, enemy.transform.position, Quaternion.identity); // Poor 파티클
                Debug.Log("Poor");
            }
            else
            {
                scoreManager.AddScore(ScoreManager.Judgement.Miss);
            }

            PlayHitSound();
            CameraShake.instance.StartShake(0.1f);
            Destroy(enemy);
        }
        else
        {
            // 적을 공격하지 못했을 경우 Miss 처리
            scoreManager.AddScore(ScoreManager.Judgement.Miss);
        }
    }

    void PlayHitSound()
    {
        if (!string.IsNullOrEmpty(hitSoundEventPath))
        {
            RuntimeManager.PlayOneShot(hitSoundEventPath);
        }
    }

    void PlaySwingSound()
    {
        if (!string.IsNullOrEmpty(swingSoundEventPath))
        {
            RuntimeManager.PlayOneShot(swingSoundEventPath);
        }
    }

    // 유니티 에디터에서 공격 범위를 시각적으로 표시
    void OnDrawGizmosSelected()
    {
        Vector2 attackPosition = (Vector2)transform.position + attackOffset + judgementOffset;

        // 공격 범위 (원형)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRange);

        // Perfect Range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition, perfectRange);

        // Great Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition, greatRange);

        // Good Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPosition, goodRange);

        // Miss Range
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(attackPosition, poorRange);
    }
}
