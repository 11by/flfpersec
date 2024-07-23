using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public Animator animator; // Animator �߰�

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
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

            // ���� ���� ���� ���� �����ϰ� ����
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, attackRange);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
