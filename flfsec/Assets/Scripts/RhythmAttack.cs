using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public Vector2 attackOffset; // ���� ������ ��ġ ������
    public LayerMask enemyLayer;
    public Animator animator; // Animator �߰�
    public PlayerController playerController; // PlayerController �߰�
    public GameObject hitParticlePrefab; // ��ƼŬ ������ �߰�

    public string hitSoundEventPath;
    private Pause pause; // Pause ��ũ��Ʈ ���� ����

    void Start()
    {
        pause = FindObjectOfType<Pause>(); // Pause ��ũ��Ʈ ����
    }

    void Update()
    {
        // �Ͻ����� ���°� �ƴ� ��쿡�� ���� �Է� ó��
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L)) && (pause == null || !pause.IsPause))
        {
            Attack();
        }
    }

    void Attack()
    {
        bool isAirborne = animator.GetBool("IsAirborne");

        // �ִϸ��̼� Ʈ���� ����
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

        // ���� ���� ���� ���� ����
        Vector2 attackPosition = (Vector2)transform.position + attackOffset;
        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPosition, Vector2.right, attackRange, enemyLayer);

        // ���� ���ʿ� �ִ� ���� ã��
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

        // ���� ���ʿ� �ִ� �� ó��
        if (leftmostHit.collider != null)
        {
            GameObject enemy = leftmostHit.collider.gameObject;

            // ���� �̸��� ���� �÷��̾��� ���� ����
            if (enemy.name.Contains("Up"))
            {
                playerController.JumpUp();
            }
            else if (enemy.name.Contains("Down"))
            {
                playerController.JumpDown();
            }

            // ��ƼŬ ����Ʈ ���
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

    // ����Ƽ �����Ϳ��� ���� ������ �ð������� ǥ��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = (Vector2)transform.position + attackOffset;
        Gizmos.DrawLine(attackPosition, attackPosition + Vector2.right * attackRange);
    }
}
