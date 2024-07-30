using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private StudioEventEmitter musicEmitter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // MusicEmitter ������Ʈ�� ã�� StudioEventEmitter ������Ʈ�� �����ɴϴ�.
        musicEmitter = GameObject.Find("MusicEmitter").GetComponent<StudioEventEmitter>();
    }

    public void SetVolume(float volume)
    {
        // ���� ���� (0.0f���� 1.0f ������ ��)
        musicEmitter.SetParameter("Volume", volume);
    }
}
