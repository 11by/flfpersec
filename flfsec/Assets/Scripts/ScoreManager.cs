using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public enum Judgement { Perfect, Great, Good, Poor, Miss }

    private int perfectCount = 0;
    private int greatCount = 0;
    private int goodCount = 0;
    private int poorCount = 0;
    private int missCount = 0;

    private int totalScore = 0;
    private int scorePerNote = 0;
    private EnemyManager enemyManager;

    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();

        // 노트의 총 수를 가져와서 노트당 점수를 계산합니다.
        int totalEnemies = enemyManager.GetTotalEnemies();
        if (totalEnemies > 0)
        {
            scorePerNote = 1000000 / totalEnemies;
        }
        else
        {
            Debug.LogWarning("No enemies found. Score per note is set to 0.");
            scorePerNote = 0;
        }
    }

    public void AddScore(Judgement judgement)
    {
        int judgementScore = 0;

        // 판정별 배율에 따라 점수 계산
        switch (judgement)
        {
            case Judgement.Perfect:
                perfectCount++;
                judgementScore = Mathf.RoundToInt(scorePerNote * 1.0f); // 100%
                break;
            case Judgement.Great:
                greatCount++;
                judgementScore = Mathf.RoundToInt(scorePerNote * 0.5f); // 50%
                break;
            case Judgement.Good:
                goodCount++;
                judgementScore = Mathf.RoundToInt(scorePerNote * 0.3f); // 30%
                break;
            case Judgement.Poor:
                poorCount++;
                judgementScore = Mathf.RoundToInt(scorePerNote * 0.01f); // 1%
                break;
            case Judgement.Miss:
                missCount++;
                judgementScore = 0; // 0%
                break;
        }

        totalScore += judgementScore;
    }

    public int GetPerfectCount() { return perfectCount; }
    public int GetGreatCount() { return greatCount; }
    public int GetGoodCount() { return goodCount; }
    public int GetPoorCount() { return poorCount; }
    public int GetMissCount() { return missCount; }
    public int GetTotalScore() { return totalScore; }
}
