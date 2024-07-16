using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    private bool IsPause;
    private bool IsResume;

    public Image pauseSprite;
    public Animator uiAnimator;

    private readonly string showTrigger = "doShow";
    private readonly string hideTrigger = "doHide";

    // Start is called before the first frame update
    void Start()
    {
        IsPause = false;   
        IsResume = false;
        pauseSprite.enabled = false;

        if (uiAnimator != null)
        {
            uiAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            uiAnimator.ResetTrigger(showTrigger);
            uiAnimator.ResetTrigger(hideTrigger);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPause)
            {
                PauseGame();
            }

            else if (IsPause && !IsResume)
            {
                StartCoroutine(ResumeGameAfterDelay(3.0f));
            }
        }
    }

    void PauseGame()
    {
        IsPause = true;
        IsResume= false;

        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger(showTrigger);
        }

        pauseSprite.enabled = true;
        Time.timeScale = 0;
    }

    IEnumerator ResumeGameAfterDelay(float delay)
    {
        IsResume = true;

        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger(hideTrigger);
        }

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;
        IsPause = false;
        IsResume = false;
        pauseSprite.enabled = false;
    }
}