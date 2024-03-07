using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private XRIDefaultInputActions action;
    [SerializeField] private bool paused;
    public List<GameObject> menus;

    [SerializeField] private bool PauseOnStart;

    [SerializeField] private bool isPauseMenu;

    private void Awake()
    {
        action = new XRIDefaultInputActions();
        menus = new List<GameObject>();

        if (isPauseMenu)
        {
            if (PauseOnStart)
            {
                Time.timeScale = 0; //Starts time 
                //AudioListener.pause = true; // turns off sound
                paused = true;
                action.Disable();
            }
            else
            {
                Time.timeScale = 1; //Starts time 
                //AudioListener.pause = false; // turns back on sound
                paused = false;
                action.Enable();
            }
        }
    }

    private void Start()
    {
        if (isPauseMenu)
        {
              if (!PauseOnStart)
                action.XRILeftHand.Menu.performed+= _ => DeterminePause();
        }


        // add all the menus to one list
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject currentMenu = transform.GetChild(i).gameObject;
            menus.Add(currentMenu);
        }
    }

    private void DeterminePause()
    {
        if (paused)
            ResumeGame();
        else
            PauseGame();
    }
    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    public void PauseGame()
    {
        //Time.timeScale = 0; //Stops time

        //AudioListener.pause = true; // turns off sound
        paused = true;

        //First menu is the PauseMenu
        menus[0].SetActive(true);
    }

    public void PauseOnGameEnd()
    {
        Time.timeScale = 0; //Stops time

        //AudioListener.pause = true; // turns off sound
        paused = true;

        //action.Disable();
    }

    public void ResumeGame()
    {
        //Time.timeScale = 1; //Starts time 
        //AudioListener.pause = false; // turns back on sound
        paused = false;

        foreach (GameObject g in menus)
            g.SetActive(false);
    }

    public void SubscribeToPause()
    {
        action.XRILeftHand.Menu.performed += _ => DeterminePause();
    }

    private int GetIndexByMenuName(Transform currentMenu, string childMenuName)
    {
        for (int i = 0; i < currentMenu.childCount; i++)
        {
            Transform currentChildMenu = currentMenu.GetChild(i);
            if (currentChildMenu.name == childMenuName)
            {
                return i;
            }
        }

        return -1;
    }
}
