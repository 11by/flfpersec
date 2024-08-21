using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // 배경의 스크롤 속도
    public float parallaxEffectMultiplier = 0.5f; // 패럴랙스 효과를 제어하는 변수
    public GameObject backgroundTilePrefab; // 배경 타일 프리팹
    public int tileCount = 3; // 화면에 표시할 타일 개수
    public float initialOffset = 0.5f; // 첫 위치를 왼쪽으로 조정할 오프셋 값

    private List<GameObject> backgroundTiles = new List<GameObject>();
    private float tileWidth;
    private Camera mainCamera;
    private float leftBoundary;

    void Start()
    {
        mainCamera = Camera.main;

        // 배경 타일을 생성하고 초기 위치를 설정
        Sprite sprite = backgroundTilePrefab.GetComponent<SpriteRenderer>().sprite;
        tileWidth = sprite.texture.width / sprite.pixelsPerUnit;

        for (int i = 0; i < tileCount; i++)
        {
            // 초기 위치를 왼쪽으로 약간 이동시켜 첫 타일의 시작 위치 조정
            Vector3 position = new Vector3(transform.position.x + i * tileWidth - initialOffset, transform.position.y, transform.position.z);
            GameObject tile = Instantiate(backgroundTilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(transform); // 생성된 타일을 현재 오브젝트의 자식으로 설정
            backgroundTiles.Add(tile);
        }
    }

    void Update()
    {
        float scrollAmount = scrollSpeed * Time.deltaTime;

        // 화면의 왼쪽 경계 계산 (카메라의 왼쪽 경계)
        leftBoundary = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - tileWidth;

        // 모든 배경 타일을 왼쪽으로 스크롤
        for (int i = 0; i < backgroundTiles.Count; i++)
        {
            backgroundTiles[i].transform.Translate(Vector3.left * scrollAmount * parallaxEffectMultiplier);

            // 타일이 화면의 왼쪽 끝을 벗어나면 오른쪽 끝으로 재배치
            if (backgroundTiles[i].transform.position.x < leftBoundary)
            {
                // 가장 오른쪽 타일의 위치를 기준으로 새 위치 계산
                GameObject lastTile = backgroundTiles[(i + tileCount - 1) % tileCount];
                backgroundTiles[i].transform.position = new Vector3(lastTile.transform.position.x + tileWidth, backgroundTiles[i].transform.position.y, backgroundTiles[i].transform.position.z);
            }
        }
    }
}
