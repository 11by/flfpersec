using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private SettingTab settingTab;

    private bool IsPause;
    private bool IsResume;
    private bool IsDelayActive;
    public bool IsSettingOpened;        // 설정 메뉴 표시 확인 변수

    public Image pauseSprite;
    public Animator uiAnimator;
    public GameObject settingsMenu;     // 설정 메뉴 UI
    public Button settingsButton;       // 설정 메뉴 열기 버튼
    public Button backButton;           // 설정 메뉴 닫기 버튼

    private readonly string showTrigger = "doShow";
    private readonly string hideTrigger = "doHide";

    void Start()
    {
        settingTab = FindObjectOfType<SettingTab>();
        IsSettingOpened = false;
        settingsMenu.SetActive(false);
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
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (backButton != null)
            backButton.onClick.AddListener(CloseSettings);
    }

    void Update()
    {
        // 지연 상태라면 아무것도 하지 않음
        if (IsDelayActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 실행 중 esc키가 눌린 경우 -> 게임 정지
            if (!IsPause)
            {
                PauseGame();
            }
            // 정지 상태이고 설정 창이 켜져 있는 경우 -> 설정창 꺼짐
            else if (IsPause && IsSettingOpened)
                CloseSettings(); // 설정창이 열려있으면 닫기

            // 정지 상태이고 설정 창이 꺼져있는 경우 -> 게임 재개
            else if (IsPause && !IsSettingOpened)
                StartCoroutine(ResumeGameAfterDelay(3.0f));

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

    public void OpenSettings()
    {
        IsSettingOpened = true;
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        IsSettingOpened = false;
        settingsMenu.SetActive(false);
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
