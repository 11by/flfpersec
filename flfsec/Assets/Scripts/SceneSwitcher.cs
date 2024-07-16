using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // ��ȯ�ϰ��� �ϴ� ���� �̸� �Ǵ� �ε����� �����մϴ�.
    public string sceneName;

    // �� �޼��带 ��ư�� OnClick �̺�Ʈ�� �����մϴ�.
    public void SwitchScene()
    {
        // ������ ������ ��ȯ�մϴ�.
        SceneManager.LoadScene(sceneName);
    }
}