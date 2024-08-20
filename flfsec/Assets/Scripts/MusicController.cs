using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    public string musicEventPath;
    private EventInstance musicInstance;

    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEventPath);
        musicInstance.start();
    }

    public void PauseMusic()
    {
        musicInstance.setPaused(true);
    }

    public void ResumeMusic()
    {
        musicInstance.setPaused(false);
    }

    public void StopMusic() // 명확한 이름의 메서드 추가
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }

    public void OnDestroy()
    {
        StopMusic(); // OnDestroy에서 StopMusic을 호출
    }
}
