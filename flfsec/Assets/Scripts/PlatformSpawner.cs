using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float scrollSpeed = 2.0f;

    public Sprite[] platformSprites; // ���� ��������Ʈ �迭 �߰�

    public GameObject SpawnPlatform(Vector3 position)
    {
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);

        // ���� ��������Ʈ ����
        int randomIndex = Random.Range(0, platformSprites.Length);
        Sprite selectedSprite = platformSprites[randomIndex];

        // ��������Ʈ�� ������ SpriteRenderer�� ����
        SpriteRenderer spriteRenderer = platform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = selectedSprite;
        }

        platform.AddComponent<PlatformScroller>().scrollSpeed = scrollSpeed;
        return platform;
    }
}
