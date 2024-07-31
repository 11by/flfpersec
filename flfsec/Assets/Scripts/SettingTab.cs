using UnityEngine;
using UnityEngine.UI;

public class SettingTab : MonoBehaviour
{
    public bool IsSettingOpened;

    public GameObject settingsMenu; // Settings Menu UI
    public Button settingsButton; // Button to open Settings Menu
    public Button backButton; // Button to go back to Pause Menu

    void Start()
    {
        IsSettingOpened = false;

        settingsMenu.SetActive(false);

        // Assign button click events
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseSettings);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsSettingOpened)
        {
            CloseSettings();
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
}
