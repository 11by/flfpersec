using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlatformManager : MonoBehaviour
{
    public PlatformSpawner platformSpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // 게임이 진행될 시간(초)
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
        beatInterval = 60.0f / beatsPerMinute; // 한 비트의 시간 (초)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // 게임 전체에서의 총 박자 수

        // 발판 프리팹의 BoxCollider2D 크기를 기반으로 platformWidth 설정
        platformWidth = platformSpawner.platformPrefab.GetComponent<BoxCollider2D>().size.x;

        for (int i = 0; i < numberOfBeats; i++)
        {
            if (platformPattern[i])
            {
                Vector3 position;

                if (platformCount == 0)
                {
                    // 첫 번째 발판의 위치를 플레이어의 중앙에 맞추기
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

        // 발판이 화면을 벗어났는지 확인
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = platforms[i];

            if (platform == null)
            {
                platforms.RemoveAt(i);
                continue;
            }

            // 발판이 플레이어의 x좌표에서 더 멀리 떨어진 후 제거되도록 수정
            if (platform.transform.position.x < player.position.x - 2 * platformWidth)
            {
                platforms.RemoveAt(i);
                Destroy(platform);
                progressBarController.UpdateProgressBar();
            }
        }
    }
}
