using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float scrollSpeed = 2.0f;

    public Sprite[] platformSprites; // 발판 스프라이트 배열 추가

    public GameObject SpawnPlatform(Vector3 position)
    {
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);

        // 랜덤 스프라이트 선택
        int randomIndex = Random.Range(0, platformSprites.Length);
        Sprite selectedSprite = platformSprites[randomIndex];

        // 스프라이트를 발판의 SpriteRenderer에 적용
        SpriteRenderer spriteRenderer = platform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = selectedSprite;
        }

        platform.AddComponent<PlatformScroller>().scrollSpeed = scrollSpeed;
        return platform;
    }
}
