using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlatformManager : MonoBehaviour
{
    public PlatformSpawner platformSpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // ������ ����� �ð�(��)
    public bool[] platformPattern;
    public StudioEventEmitter musicEmitter;
    public ProgressBarController progressBarController;
    public Transform player;

    private float beatInterval;
    private int numberOfBeats;
    private List<GameObject> platforms = new List<GameObject>();
    private int platformCount = 0;
    private float platformWidth;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // �� ��Ʈ�� �ð� (��)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // ���� ��ü������ �� ���� ��

        // ���� �������� BoxCollider2D ũ�⸦ ������� platformWidth ����
        platformWidth = platformSpawner.platformPrefab.GetComponent<BoxCollider2D>().size.x;

        for (int i = 0; i < numberOfBeats; i++)
        {
            if (platformPattern[i])
            {
                Vector3 position;

                if (platformCount == 0)
                {
                    // ù ��° ������ ��ġ�� �÷��̾��� �߾ӿ� ���߱�
                    position = new Vector3(player.position.x, -4, 0);
                }
                else
                {
                    position = new Vector3(player.position.x + i * beatInterval * scrollSpeed, -4, 0);
                }

                GameObject platform = platformSpawner.SpawnPlatform(position);
                platforms.Add(platform);

                platformCount++;

                if (platformCount == 9)
                {
                    musicEmitter.Play();
                }
            }
        }
    }

    void Update()
    {
        float scrollAmount = scrollSpeed * Time.deltaTime;

        // ������ ȭ���� ������� Ȯ��
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = platforms[i];

            if (platform == null)
            {
                platforms.RemoveAt(i);
                continue;
            }

            // ������ �÷��̾��� x��ǥ���� �� �ָ� ������ �� ���ŵǵ��� ����
            if (platform.transform.position.x < player.position.x - 2 * platformWidth)
            {
                platforms.RemoveAt(i);
                Destroy(platform);
                progressBarController.UpdateProgressBar();
            }
        }
    }
}
