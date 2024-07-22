using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar;
    public int totalPlatforms = 100; // �� ���� ����
    private int platformsPassed = 0; // ������ ���� ����

    void Start()
    {
        // �ʱ�ȭ
        progressBar.value = 0;
        progressBar.maxValue = 100; // Progress bar�� �ִ밪 ����
    }

    public void UpdateProgressBar()
    {
        // ���� ������ ���� Progress Bar ������Ʈ
        platformsPassed++;
        float progress = (float)platformsPassed / totalPlatforms * 100;
        progressBar.value = progress;
    }
}