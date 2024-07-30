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
            DontDestroyOnLoad(gameObject); // 씬 전환 시 오브젝트 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // MusicEmitter 오브젝트를 찾고 StudioEventEmitter 컴포넌트를 가져옵니다.
        musicEmitter = GameObject.Find("MusicEmitter").GetComponent<StudioEventEmitter>();
    }

    public void SetVolume(float volume)
    {
        // 볼륨 설정 (0.0f에서 1.0f 사이의 값)
        musicEmitter.SetParameter("Volume", volume);
    }
}
