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
    public string swingSoundEventPath;
    private Pause pause; // Pause ��ũ��Ʈ ���� ����
    private ScoreManager scoreManager; // ScoreManager ���� ����

    [Header("Judgement Ranges")]
    public float perfectRange = 0.5f;
    public float greatRange = 1.0f;
    public float goodRange = 1.5f;
    public float missRange = 2.0f;

    [Header("Judgement Offset")]
    public Vector2 judgementOffset = Vector2.zero; // ���� ������ �߽� ������

    void Start()
    {
        pause = FindObjectOfType<Pause>(); // Pause ��ũ��Ʈ ����
        scoreManager = FindObjectOfType<ScoreManager>(); // ScoreManager ��ũ��Ʈ ����
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
        PlaySwingSound();

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
        Vector2 attackPosition = (Vector2)transform.position + attackOffset + judgementOffset;
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
            float distanceToPlayer = Mathf.Abs(leftmostHit.point.x - transform.position.x);

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

            // ������ ���� ���� ó��
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
            // ���� �������� ������ ��� Miss ó��
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

    // ����Ƽ �����Ϳ��� ���� ������ �ð������� ǥ��
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
