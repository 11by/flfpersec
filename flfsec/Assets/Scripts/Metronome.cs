using FMODUnity;
using UnityEngine;
public class Metronome : MonoBehaviour
{
    public EventReference metronomeEvent;

    void Start()
    {
        RuntimeManager.PlayOneShot(metronomeEvent);
    }
}