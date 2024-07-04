using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformSpawner platformSpawner;
    public float scrollSpeed = 2.0f;
    public float beatsPerMinute = 120.0f; // BPM
    public float durationInSeconds = 30.0f; // ������ ����� �ð�(��)
    public bool[] platformPattern;

    private float beatInterval;
    private int numberOfBeats;

    void Start()
    {
        beatInterval = 60.0f / beatsPerMinute; // �� ��Ʈ�� �ð� (��)
        numberOfBeats = Mathf.FloorToInt(durationInSeconds / beatInterval); // ���� ��ü������ �� ���� ��

        for (int i = 0; i < numberOfBeats; i++)
        {
            if (platformPattern[i])
            {
                Vector3 position = new Vector3(i * beatInterval * scrollSpeed, -4, 0);
                platformSpawner.SpawnPlatform(position);
            }
        }
    }
}
