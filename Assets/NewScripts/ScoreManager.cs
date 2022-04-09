using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static int Score;
    public int score;
    public int highScore;
    static float Average;
    public int rankingScore;
    public Text scoreText;
    public Text highScoreText;
    public Text averageText;
    public Text miniGame;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("SCORE", 0);
    }

    public void ScoreCount()
    {
        Score++;
        miniGame.text = "SCORE:" + Score.ToString("00") + "\r\n" + "HIGHSCORE:" + highScore.ToString("00");
    }

    public void SetScore()
    {
        miniGame.text = "SCORE:" + Score.ToString("00") + "\r\n" + "HIGHSCORE:" + highScore.ToString("00");
    }

    public void SaveScore()
    {
        score = Score;

        if (highScore < score)
        {
            highScore = score;

            PlayerPrefs.SetInt("SCORE", highScore);
            PlayerPrefs.Save();
        }

        scoreText.text = "Score：" + score.ToString("00");
        highScoreText.text = "HighScore：" + highScore.ToString("00");
        averageText.text = "Average：" + Average.ToString("0.0") + "回/秒";
    }

    public void RankingSet()
    {
        rankingScore = Score;
    }

    public void DelScore()
    {
        PlayerPrefs.DeleteKey("SCORE");
        highScoreText.text = "HighScore：00";
    }

    public void AverageInput()
    {
        GameObject miniGame = GameObject.Find("MiniGameManager");
        Average = miniGame.GetComponent<MiniGameManager>().average;
    }

    public void DelStatic()
    {
        Score = 0;
        Average = 0;
    }
}
