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

    void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}
