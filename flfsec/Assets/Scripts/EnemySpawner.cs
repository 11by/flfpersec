using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float scrollSpeed = 2.0f;

    public void SpawnEnemy(Vector3 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.AddComponent<EnemyScroller>().scrollSpeed = scrollSpeed;
        float enemyLength = 1f; // 발판의 길이 설정 (원하는 크기로 조정)
        enemy.transform.localScale = new Vector3(enemyLength, 1, 1);
    }
}