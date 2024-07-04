using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmAttack : MonoBehaviour
{
    public float attackInterval = 0.5f;
    private float attackTimer = 0;
    public float attackRange = 1f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
            {
                Attack();
                attackTimer = 0;
            }
        }
    }

    void Attack()
    {
        // 공격 범위 내의 적을 감지하고 제거
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
