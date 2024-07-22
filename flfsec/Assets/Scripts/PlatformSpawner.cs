using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float scrollSpeed = 2.0f;

    public GameObject SpawnPlatform(Vector3 position)
    {
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);
        platform.AddComponent<PlatformScroller>().scrollSpeed = scrollSpeed;
        return platform;
    }
}