using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float spawnInterval = 1.0f;
    public float scrollSpeed = 2.0f;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPlatform();
            timer = 0;
        }
    }
    void SpawnPlatform()
    {
        GameObject platform = Instantiate(platformPrefab, new Vector3(10, -2, 0), Quaternion.identity);
        platform.AddComponent<PlatformScroller>().scrollSpeed = scrollSpeed;
    }
}