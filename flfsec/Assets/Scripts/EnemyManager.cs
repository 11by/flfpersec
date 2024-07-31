using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // 게임이 진행될 시간(초)
    public EnemyType[] enemyPattern; // 적이 나타날 박자 패턴 (적의 종류와 y좌표)
    public float jumpHeight; // 점프 높이
    public Transform player;

    private float beatInterval;
    private int numberOfBeats;
    private int enemyCount = 0;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // 한 비트의 시간 (초)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // 게임 전체에서의 총 박자 수

        for (int i = 0; i < numberOfBeats; i++)
        {
            if (enemyPattern[i] != null && enemyPattern[i].spawn)
            {
                float yPos = -2 + (enemyPattern[i].yPosition * jumpHeight);
                Vector3 position;

                if (enemyCount == 0)
                {
                    // 첫 번째 적의 위치를 플레이어의 중앙에 맞추기
                    position = new Vector3(player.position.x, yPos, 0);
                }
                else
                {
                    position = new Vector3(player.position.x + i * beatInterval * scrollSpeed, yPos, 0);
                }

                enemySpawner.SpawnEnemy(position, enemyPattern[i].type);
                enemyCount++;
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
    public bool spawn; // 적이 나타날지 여부
    public string type; // 적의 종류 ("Normal", "Up", "Down")
    public float yPosition; // 적의 y좌표 (Jump Height 단위)
}
