using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int lobbyIndex = 0;
    private int level1Index = 1;
    private int level2Index = 2;
    private int level3Index = 3;
    void Start()
    {
        
    }

    public void StartMenu()
    {
        SceneManager.LoadScene(lobbyIndex);
    }
    public void StartLevel1()
    {
        SceneManager.LoadScene(level1Index);
    }
    public void StartLevel2()
    {
        SceneManager.LoadScene(level2Index);
    }
    public void StartLevel3()
    {
        SceneManager.LoadScene(level3Index);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
