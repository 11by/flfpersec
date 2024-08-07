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
    private Pause pause; // Pause 스크립트 참조 변수

    void Start()
    {
        pause = FindObjectOfType<Pause>(); // Pause 스크립트 참조
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
        Vector2 attackPosition = (Vector2)transform.position + attackOffset;
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
            Destroy(enemy);
        }
    }

    void PlayHitSound()
    {
        if (!string.IsNullOrEmpty(hitSoundEventPath))
        {
            RuntimeManager.PlayOneShot(hitSoundEventPath);
        }
    }

    // 유니티 에디터에서 공격 범위를 시각적으로 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = (Vector2)transform.position + attackOffset;
        Gizmos.DrawLine(attackPosition, attackPosition + Vector2.right * attackRange);
    }
}
