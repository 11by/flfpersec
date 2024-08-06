using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class RhythmAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public Vector2 attackOffset;        // 공격 범위의 위치 오프셋
    public LayerMask enemyLayer;
    public Animator animator;           // Animator 추가
    public PlayerController playerController;   // PlayerController 추가
    public string hitSoundEventPath;    // FMOD 이벤트 경로
    private Pause pause;        // 정지 기능 인스턴스

    void Start()
    {
        // 정지 시 공격하지 못하도록 하기 위해 Pause 인스턴스를 가져옴
        pause = FindObjectOfType<Pause>();
    }

    void Update()
    {
        // 게임이 정지 상태가 아니며, D, F, K, L을 눌렀을 경우 -> 캐릭터 공격
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L)) && (pause == null || pause.IsPause == false))
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

            // 효과음 재생
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
