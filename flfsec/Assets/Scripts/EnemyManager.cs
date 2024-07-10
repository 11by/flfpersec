using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // 게임이 진행될 시간(초)
    public bool[] enemyPattern; // 적이 나타날 박자 패턴
    public float[] enemyYPositions;

    private float beatInterval;
    private int numberOfBeats;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // 한 비트의 시간 (초)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // 게임 전체에서의 총 박자 수

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