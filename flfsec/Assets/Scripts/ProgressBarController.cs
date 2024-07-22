using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar;
    public int totalPlatforms = 100; // 총 발판 개수
    private int platformsPassed = 0; // 지나간 발판 개수

    void Start()
    {
        // 초기화
        progressBar.value = 0;
        progressBar.maxValue = 100; // Progress bar의 최대값 설정
    }

    public void UpdateProgressBar()
    {
        // 발판 개수에 따라 Progress Bar 업데이트
        platformsPassed++;
        float progress = (float)platformsPassed / totalPlatforms * 100;
        progressBar.value = progress;
    }
}