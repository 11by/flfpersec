using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // ������ ����� �ð�(��)
    public bool[] enemyPattern; // ���� ��Ÿ�� ���� ����
    public float[] enemyYPositions;

    private float beatInterval;
    private int numberOfBeats;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // �� ��Ʈ�� �ð� (��)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // ���� ��ü������ �� ���� ��

        for (int i = 0; i < numberOfBeats; i++)
        {
            if (enemyPattern[i])
            {
                Vector3 position = new Vector3(i * beatInterval * scrollSpeed, -2 + enemyYPositions[i], 0);
                enemySpawner.SpawnEnemy(position);
            }
        }
    }

    void SpawnEnemy(Vector2 position)
    {
        Instantiate(enemySpawner, position, Quaternion.identity);
    }
}