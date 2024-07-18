using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // ��ȯ�ϰ��� �ϴ� ���� �̸� �Ǵ� �ε����� �����մϴ�.
    public string sceneName;

    // �� �޼��带 ��ư�� OnClick �̺�Ʈ�� �����մϴ�.

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;

        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }

    public void SwitchScene()
    {
        Time.timeScale = 1;

        // ������ ������ ��ȯ�մϴ�.
        SceneManager.LoadScene(sceneName);
    }
}