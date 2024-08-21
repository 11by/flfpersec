using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // ��ȯ�ϰ��� �ϴ� ���� �̸� �Ǵ� �ε����� �����մϴ�.
    public string sceneName;
    private MusicController musicController;

    // �� �޼��带 ��ư�� OnClick �̺�Ʈ�� �����մϴ�.

    void Start()
    {
        musicController = FindObjectOfType<MusicController>();
    }

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;

        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);

        if (musicController != null)
        {
            musicController.ResumeMusic();
            musicController.StopMusic();
            musicController.StartMusic();
        }
    }

    public void SwitchScene()
    {
        Time.timeScale = 1;

        // ������ ������ ��ȯ�մϴ�.
        SceneManager.LoadScene(sceneName);
    }
}