using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    [SerializeField] private Text tipText;
    [SerializeField] private GameObject tipActive;
    [SerializeField] private GameObject Zone1;
    [SerializeField] private GameObject Zone2;
    [SerializeField] private GameObject Zone3;
    public int popupTimeCounter;
    public int count = 260;
    private bool popupActive = false;
    private int time = 0;
    private int Textnumber = 1;
    public string[] text1;
    void Start()
    {       
        
    }
    void Update()
    {
        if (popupActive) 
        {
            if (time == count)
            {
                ShowPopup(text1[Textnumber]);
                Textnumber++;
                time = 0;
            }
            else 
            { 
                time++; 
            }
        }             
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the collider is the player
        if (other.CompareTag("Player"))
        {
            // Activate the object if it's not already active
            if (tipActive != null && !tipActive.activeSelf)
            {
                popupActive = true;
                tipActive.SetActive(true);
                ShowPopup(text1[0]);
                Debug.Log("Object activated!");
                Zone2.SetActive(false);
                Zone3.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the collider is the player
        if (other.CompareTag("Player"))
        {
            // Deactivate the object if it's active
            if (tipActive != null && tipActive.activeSelf)
            {
                Textnumber = 1;
                tipActive.SetActive(false);
                HidePopup();
                Debug.Log("Object deactivated!");
                Zone2.SetActive(true);
                Zone3.SetActive(true);
            }
        }
    }
    // Show the popup with the given text
    public void ShowPopup(string text)
    {
        tipText.text = text;
        tipActive.SetActive(true);
    }

    // Hide the popup
    public void HidePopup()
    {
        tipActive.SetActive(false);
    }
}
