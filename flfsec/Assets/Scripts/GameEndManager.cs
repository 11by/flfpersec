using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    public ScoreManager scoreManager;

    public GameObject resultPanel; // 결과 패널
    public Text resultPerfectText;
    public Text resultGreatText;
    public Text resultGoodText;
    public Text resultPoorText;
    public Text resultMissText;
    public Text resultScoreText;


    void Start()
    {
        resultPanel.SetActive(false);
    }

    void Update()
    {
        
    }
    public void ShowResults()
    {
        resultPanel.SetActive(true);

        resultPerfectText.text = "Perfect: " + scoreManager.GetPerfectCount();
        resultGreatText.text = "Great: " + scoreManager.GetGreatCount();
        resultGoodText.text = "Good: " + scoreManager.GetGoodCount();
        resultPoorText.text = "Poor: " + scoreManager.GetPoorCount();
        resultMissText.text = "Miss: " + scoreManager.GetMissCount();
        resultScoreText.text = "Final Score: " + scoreManager.GetTotalScore();
    }
}
