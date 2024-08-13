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

    [Header("Particle Effects")]
    public GameObject perfectParticlePrefab; // Perfect ���� ��ƼŬ ������
    public GameObject greatParticlePrefab;   // Great ���� ��ƼŬ ������
    public GameObject goodParticlePrefab;    // Good ���� ��ƼŬ ������
    public GameObject poorParticlePrefab;    // Poor ���� ��ƼŬ ������

    public string hitSoundEventPath;
    public string swingSoundEventPath;
    private Pause pause; // Pause ��ũ��Ʈ ���� ����
    private ScoreManager scoreManager; // ScoreManager ���� ����

    [Header("Judgement Ranges")]
    float perfectRange = 0.5f;
    float greatRange = 1.0f;
    float goodRange = 1.5f;
    float poorRange = 2.0f;

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
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayer);

        // ���� ���ʿ� �ִ� ���� ã��
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

        // ���� ���ʿ� �ִ� �� ó��
        if (leftmostHit != null)
        {
            GameObject enemy = leftmostHit.gameObject;
            float distanceToPlayer = Vector2.Distance(leftmostHit.transform.position, transform.position);

            // ���� �̸��� ���� �÷��̾��� ���� ����
            if (enemy.name.Contains("Up"))
            {
                playerController.JumpUp();
            }
            else if (enemy.name.Contains("Down"))
            {
                playerController.JumpDown();
            }

            // ������ ���� ���� ó�� �� ��ƼŬ ����Ʈ ���
            distanceToPlayer -= 1.4f;
            if (distanceToPlayer <= perfectRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Perfect);
                Instantiate(perfectParticlePrefab, enemy.transform.position, Quaternion.identity); // Perfect ��ƼŬ
                Debug.Log("Perfect");
            }
            else if (distanceToPlayer <= greatRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Great);
                Instantiate(greatParticlePrefab, enemy.transform.position, Quaternion.identity); // Great ��ƼŬ
                Debug.Log("Great");
            }
            else if (distanceToPlayer <= goodRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Good);
                Instantiate(goodParticlePrefab, enemy.transform.position, Quaternion.identity); // Good ��ƼŬ
                Debug.Log("Good");
            }
            else if (distanceToPlayer <= poorRange)
            {
                scoreManager.AddScore(ScoreManager.Judgement.Poor);
                Instantiate(poorParticlePrefab, enemy.transform.position, Quaternion.identity); // Poor ��ƼŬ
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
        Vector2 attackPosition = (Vector2)transform.position + attackOffset + judgementOffset;

        // ���� ���� (����)
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
