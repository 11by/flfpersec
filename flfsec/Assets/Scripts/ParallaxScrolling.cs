using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // ����� ��ũ�� �ӵ�
    public float parallaxEffectMultiplier = 0.5f; // �з����� ȿ���� �����ϴ� ����
    public GameObject backgroundTilePrefab; // ��� Ÿ�� ������
    public int tileCount = 3; // ȭ�鿡 ǥ���� Ÿ�� ����
    public float initialOffset = 0.5f; // ù ��ġ�� �������� ������ ������ ��

    private List<GameObject> backgroundTiles = new List<GameObject>();
    private float tileWidth;
    private Camera mainCamera;
    private float leftBoundary;

    void Start()
    {
        mainCamera = Camera.main;

        // ��� Ÿ���� �����ϰ� �ʱ� ��ġ�� ����
        Sprite sprite = backgroundTilePrefab.GetComponent<SpriteRenderer>().sprite;
        tileWidth = sprite.texture.width / sprite.pixelsPerUnit;

        for (int i = 0; i < tileCount; i++)
        {
            // �ʱ� ��ġ�� �������� �ణ �̵����� ù Ÿ���� ���� ��ġ ����
            Vector3 position = new Vector3(transform.position.x + i * tileWidth - initialOffset, transform.position.y, transform.position.z);
            GameObject tile = Instantiate(backgroundTilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(transform); // ������ Ÿ���� ���� ������Ʈ�� �ڽ����� ����
            backgroundTiles.Add(tile);
        }
    }

    void Update()
    {
        float scrollAmount = scrollSpeed * Time.deltaTime;

        // ȭ���� ���� ��� ��� (ī�޶��� ���� ���)
        leftBoundary = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - tileWidth;

        // ��� ��� Ÿ���� �������� ��ũ��
        for (int i = 0; i < backgroundTiles.Count; i++)
        {
            backgroundTiles[i].transform.Translate(Vector3.left * scrollAmount * parallaxEffectMultiplier);

            // Ÿ���� ȭ���� ���� ���� ����� ������ ������ ���ġ
            if (backgroundTiles[i].transform.position.x < leftBoundary)
            {
                // ���� ������ Ÿ���� ��ġ�� �������� �� ��ġ ���
                GameObject lastTile = backgroundTiles[(i + tileCount - 1) % tileCount];
                backgroundTiles[i].transform.position = new Vector3(lastTile.transform.position.x + tileWidth, backgroundTiles[i].transform.position.y, backgroundTiles[i].transform.position.z);
            }
        }
    }
}
