using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class ScoreEntry
{
    public float score;
    public string levelName;

    public ScoreEntry(float score, string levelName)
    {
        this.score = score;
        this.levelName = levelName;
    }
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public List<ScoreEntry> scores = new List<ScoreEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(float score, string levelName)
    {
        scores.Add(new ScoreEntry(score, levelName));
    }

    public void SaveScores()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/scoredata.dat";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, scores);
            Debug.Log("Scores saved to " + path);
        }
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/scoredata.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                scores = formatter.Deserialize(stream) as List<ScoreEntry>;
                Debug.Log("Scores loaded from " + path);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path + ", creating a new one.");
            SaveEmptyScores();
        }
    }

    private void SaveEmptyScores()
    {
        scores = new List<ScoreEntry>();
        SaveScores();
    }
}