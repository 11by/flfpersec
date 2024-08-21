using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // ������ ����� �ð�(��)
    public EnemyType[] enemyPattern; // ���� ��Ÿ�� ���� ���� (���� ������ y��ǥ)
    public float jumpHeight; // ���� ����
    public Transform player;
    public float positionOffset = 0.0f; // �߰�: �� ��ġ ������

    private float beatInterval;
    private int numberOfBeats;
    public int enemyCount = 0;
    private ScoreManager scoreManager;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // �� ��Ʈ�� �ð� (��)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // ���� ��ü������ �� ���� ��
        scoreManager = FindObjectOfType<ScoreManager>();

        for (int i = 0; i < numberOfBeats; i++)
        {
            // ù ��° ��Ʈ���� ���� �������� ����
            if (i == 0)
            {
                continue;
            }

            if (enemyPattern[i] != null && enemyPattern[i].spawn)
            {
                float yPos = -2 + (enemyPattern[i].yPosition * jumpHeight);

                // ������ ����
                Vector3 position = new Vector3(player.position.x + i * beatInterval * scrollSpeed + positionOffset, yPos, 0);

                GameObject enemy = enemySpawner.SpawnEnemy(position, enemyPattern[i].type);
                ApplyAnimationToEnemy(enemy, yPos);
                enemyCount++;
            }
        }
    }

    void ApplyAnimationToEnemy(GameObject enemy, float yPos)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            if (yPos > -2)
            {
                enemyScript.SetAnimation("EnemAir");
            }
            else
            {
                enemyScript.SetAnimation("EnemGround");
            }
        }
    }

    // ���� �� �� ��ȯ
    public int GetTotalEnemies()
    {
        return enemyCount;
    }
}

[System.Serializable]
public class EnemyType
{
    public bool spawn; // ���� ��Ÿ���� ����
    public string type; // ���� ���� ("Normal", "Up", "Down")
    public float yPosition; // ���� y��ǥ (Jump Height ����)
}
