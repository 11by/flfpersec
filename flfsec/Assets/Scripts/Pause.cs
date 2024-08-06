using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public bool IsPause;
    private bool IsDelayActive;
    public bool IsSettingOpened;        // 설정 메뉴 표시 확인 변수

    public Image pauseSprite;
    public Animator uiAnimator;
    public GameObject settingsMenu;     // 설정 메뉴 UI
    public Button settingsButton;       // 설정 메뉴 열기 버튼
    public Button backButton;           // 설정 메뉴 닫기 버튼
    public Button resumeButton;         // 게임 재개 버튼

    private readonly string showTrigger = "doShow";
    private readonly string hideTrigger = "doHide";

    void Start()
    {
        IsSettingOpened = false;
        settingsMenu.SetActive(false);
        IsPause = false;
        IsDelayActive = false;
        pauseSprite.enabled = false;

        if (uiAnimator != null)
        {
            uiAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            uiAnimator.ResetTrigger(showTrigger);
            uiAnimator.ResetTrigger(hideTrigger);
        }
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseSettings);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(() => {
                if (!IsDelayActive)
                {
                    StartCoroutine(ResumeGameAfterDelay(3.0f));
                }
            });
        }
    }

    void Update()
    {
        // 지연 상태라면 아무것도 하지 않음
        if (IsDelayActive)
            return;

        // Esc 키가 눌린 경우
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPause)
            {
                PauseGame();
            }
            else if (IsSettingOpened)
            {
                CloseSettings(); // 설정창이 열려있으면 닫기
            }
            else
            {
                StartCoroutine(ResumeGameAfterDelay(3.0f)); // 게임 재개
            }
        }
    }

    void PauseGame()
    {
        IsPause = true;

        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger(showTrigger); // UI 애니메이션 트리거
        }

        pauseSprite.enabled = true;
        Time.timeScale = 0;
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
        IsDelayActive = true;

        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger(hideTrigger);
        }

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;
        IsPause = false;
        IsDelayActive = false;
        pauseSprite.enabled = false;
    }
}
