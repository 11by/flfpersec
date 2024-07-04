using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float scrollSpeed = 2.0f;

    public void SpawnPlatform(Vector3 position)
    {
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);
        platform.AddComponent<PlatformScroller>().scrollSpeed = scrollSpeed;
        float platformLength = 1f; // 발판의 길이 설정 (원하는 크기로 조정)
        platform.transform.localScale = new Vector3(platformLength, 1, 1);
    }
}