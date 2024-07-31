using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private SettingTab settingTab;

    private bool IsPause;
    private bool IsResume;
    private bool IsDelayActive;

    public Image pauseSprite;
    public Animator uiAnimator;

    private readonly string showTrigger = "doShow";
    private readonly string hideTrigger = "doHide";

    void Start()
    {
        settingTab = FindObjectOfType<SettingTab>();

        IsPause = false;
        IsResume = false;
        IsDelayActive = false;
        pauseSprite.enabled = false;

        if (uiAnimator != null)
        {
            uiAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            uiAnimator.ResetTrigger(showTrigger);
            uiAnimator.ResetTrigger(hideTrigger);
        }
    }

    void Update()
    {
        if (IsDelayActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingTab.IsSettingOpened)
            {
                settingTab.CloseSettings();
            }
            else if (!IsPause)
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
        IsResume = false;

        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger(showTrigger);
        }

        pauseSprite.enabled = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (IsPause && !IsResume)
        {
            StartCoroutine(ResumeGameAfterDelay(3.0f));
        }
    }

    IEnumerator ResumeGameAfterDelay(float delay)
    {
        IsResume = true;
        IsDelayActive = true;

        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger(hideTrigger);
        }

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;
        IsPause = false;
        IsResume = false;
        IsDelayActive = false;
        pauseSprite.enabled = false;
    }
}
