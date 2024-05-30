using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Text scoreText;

    //void Start()
    //{
    //    ScoreManager.Instance.LoadScores();
    //    DisplayScores();
    //}
    void OnEnable()
    {
        // Register the callback when the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unregister the callback to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the scoreboard whenever the scene is loaded
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
