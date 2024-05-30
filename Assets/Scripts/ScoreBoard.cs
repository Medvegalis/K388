using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Text scoreText;

    void Start()
    {
        ScoreManager.Instance.LoadScores();
        DisplayScores();
    }

    void DisplayScores()
    {
        List<ScoreEntry> scores = ScoreManager.Instance.scores;
        scores.Sort((x, y) => y.score.CompareTo(x.score));

        scoreText.text = "Scores:\n";
        foreach (ScoreEntry entry in scores)
        {
            scoreText.text += $"Score: {entry.score}, Level: {entry.levelName}\n";
        }
    }
}
