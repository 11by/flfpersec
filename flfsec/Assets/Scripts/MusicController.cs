using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    public string musicEventPath;
    private EventInstance musicInstance;
    private bool isMusicPlaying;

    void Start()
    {
        if (musicInstance.isValid())
        {
            musicInstance.release();
        }

        musicInstance = RuntimeManager.CreateInstance(musicEventPath);
        StartMusic();
    }

    public void StartMusic()
    {
        if (!isMusicPlaying)
        {
            musicInstance.start();
            isMusicPlaying = true;
        }
    }

    public void PauseMusic()
    {
        if (isMusicPlaying)
        {
            musicInstance.setPaused(true);
        }
    }

    public void ResumeMusic()
    {
        if (isMusicPlaying)
        {
            musicInstance.setPaused(false);
        }
    }

    public void StopMusic()
    {
        if (isMusicPlaying)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isMusicPlaying = false;
        }
    }

    public void OnDestroy()
    {
        StopMusic(); // OnDestroy에서 StopMusic을 호출
        if (musicInstance.isValid())
        {
            musicInstance.release();
        }
    }
}
