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
    public GameObject hitParticlePrefab; // 파티클 프리팹 추가

    public string hitSoundEventPath;
    public string swingSoundEventPath;
    private Pause pause; // Pause 스크립트 참조 변수
    private ScoreManager scoreManager; // ScoreManager 참조 변수

    [Header("Judgement Ranges")]
    public float perfectRange = 0.5f;
    public float greatRange = 1.0f;
    public float goodRange = 1.5f;
    public float missRange = 2.0f;

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
        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPosition, Vector2.right, attackRange, enemyLayer);

        // 가장 왼쪽에 있는 적을 찾음
        RaycastHit2D leftmostHit = new RaycastHit2D();
        float leftmostX = Mathf.Infinity;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                if (hit.point.x < leftmostX)
                {
                    leftmostX = hit.point.x;
                    leftmostHit = hit;
                }
            }
        }

        // 가장 왼쪽에 있는 적 처리
        if (leftmostHit.collider != null)
        {
            GameObject enemy = leftmostHit.collider.gameObject;
            float distanceToPlayer = Mathf.Abs(leftmostHit.point.x - transform.position.x);

            // 적의 이름에 따른 플레이어의 동작 설정
            if (enemy.name.Contains("Up"))
            {
                playerController.JumpUp();
            }
            else if (enemy.name.Contains("Down"))
            {
                playerController.JumpDown();
            }

            // 파티클 이펙트 재생
            Instantiate(hitParticlePrefab, enemy.transform.position, Quaternion.identity);
            PlayHitSound();

            CameraShake.instance.StartShake(0.1f);

            // 판정에 따른 점수 처리
            if (distanceToPlayer <= perfectRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Perfect);
                Debug.Log("P");
            }
            else if (distanceToPlayer <= greatRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Great);
                Debug.Log("G");
            }
            else if (distanceToPlayer <= goodRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Good);
                Debug.Log("O");
            }
            else if (distanceToPlayer <= missRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Poor);
                Debug.Log("R");
            }
            else
            {
                scoreManager.AddScore(ScoreManager.Judgement.Miss);
                Debug.Log("M");
            }

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
        Gizmos.color = Color.red;
        Vector2 attackPosition = (Vector2)transform.position + attackOffset + judgementOffset;

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
        Gizmos.DrawWireSphere(attackPosition, missRange);
    }
}
