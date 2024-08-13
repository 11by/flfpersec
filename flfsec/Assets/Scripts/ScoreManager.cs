using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public enum Judgement { Perfect, Great, Good, Poor, Miss }

    public int perfectScore = 1000000; // 모든 판정을 Perfect로 내었을 때 100만점
    private int currentScore = 0;

    private int perfectCount = 0;
    private int greatCount = 0;
    private int goodCount = 0;
    private int poorCount = 0;
    private int missCount = 0;

    private int perfectPoints;
    private int greatPoints;
    private int goodPoints;
    private int poorPoints;

    void Start()
    {
        perfectPoints = perfectScore;
        greatPoints = perfectPoints / 2;
        goodPoints = perfectPoints / 3;
        poorPoints = perfectPoints / 10;
    }

    public void AddScore(Judgement judgement)
    {
        switch (judgement)
        {
            case Judgement.Perfect:
                currentScore += perfectPoints;
                perfectCount++;
                break;
            case Judgement.Great:
                currentScore += greatPoints;
                greatCount++;
                break;
            case Judgement.Good:
                currentScore += goodPoints;
                goodCount++;
                break;
            case Judgement.Poor:
                currentScore += poorPoints;
                poorCount++;
                break;
            case Judgement.Miss:
                missCount++;
                break;
        }
    }

    public void ShowResults()
    {
        Debug.Log("Final Score: " + currentScore);
        Debug.Log("Perfect: " + perfectCount);
        Debug.Log("Great: " + greatCount);
        Debug.Log("Good: " + goodCount);
        Debug.Log("Poor: " + poorCount);
        Debug.Log("Miss: " + missCount);
    }
}
