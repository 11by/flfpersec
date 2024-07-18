using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // 전환하고자 하는 씬의 이름 또는 인덱스를 지정합니다.
    public string sceneName;

    // 이 메서드를 버튼의 OnClick 이벤트에 연결합니다.

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;

        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }

    public void SwitchScene()
    {
        Time.timeScale = 1;

        // 지정된 씬으로 전환합니다.
        SceneManager.LoadScene(sceneName);
    }
}