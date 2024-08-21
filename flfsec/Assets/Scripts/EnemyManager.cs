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
    public float positionOffset = 0.0f; // 추가: 적 위치 오프셋

    private float beatInterval;
    private int numberOfBeats;
    public int enemyCount = 0;
    private ScoreManager scoreManager;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // 한 비트의 시간 (초)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // 게임 전체에서의 총 박자 수
        scoreManager = FindObjectOfType<ScoreManager>();

        for (int i = 0; i < numberOfBeats; i++)
        {
            // 첫 번째 비트에서 적을 생성하지 않음
            if (i == 0)
            {
                continue;
            }

            if (enemyPattern[i] != null && enemyPattern[i].spawn)
            {
                float yPos = -2 + (enemyPattern[i].yPosition * jumpHeight);

                // 오프셋 적용
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

    // 적의 총 수 반환
    public int GetTotalEnemies()
    {
        return enemyCount;
    }
}

[System.Serializable]
public class EnemyType
{
    public bool spawn; // 적이 나타날지 여부
    public string type; // 적의 종류 ("Normal", "Up", "Down")
    public float yPosition; // 적의 y좌표 (Jump Height 단위)
}
