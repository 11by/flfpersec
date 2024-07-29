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

    private float beatInterval;
    private int numberOfBeats;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // �� ��Ʈ�� �ð� (��)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // ���� ��ü������ �� ���� ��

        for (int i = 0; i < numberOfBeats; i++)
        {
            if (enemyPattern[i] != null && enemyPattern[i].spawn)
            {
                float yPos = -2 + (enemyPattern[i].yPosition * jumpHeight);
                Vector3 position = new Vector3(i * beatInterval * scrollSpeed, yPos, 0);
                enemySpawner.SpawnEnemy(position, enemyPattern[i].type);
            }
        }
    }

    void SpawnEnemy(Vector2 position)
    {
        Instantiate(enemySpawner, position, Quaternion.identity);
    }
}

[System.Serializable]
public class EnemyType
{
    public bool spawn; // ���� ��Ÿ���� ����
    public string type; // ���� ���� ("Normal", "Up", "Down")
    public float yPosition; // ���� y��ǥ (Jump Height ����)
}
