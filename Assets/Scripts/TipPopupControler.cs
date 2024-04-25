using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPopupControler : MonoBehaviour
{
    [SerializeField] private Text tipText;
    [SerializeField] private VolumeDetection volumeDetection;
    [SerializeField] private LookTimers lookTimers;
    [SerializeField] private SpeechToText wordCount;
    [SerializeField] private GameObject tipActive;
    public int popupTimeCounter;
    public bool ifShowPopups = true;
    private int count=0;
    private bool popupActive = false;
    private bool[] ifPopupShownAlready;
    private int wait = 0;
    void Start()
    {
        ifPopupShownAlready = new bool[4];
    }
    void Update()
    {
        if (!ifShowPopups)
            return;
        if (wait >= popupTimeCounter/2)
        {
            if (!popupActive)
            {
                if (volumeDetection.avarageVolume < 4 && !ifPopupShownAlready[0])
                {
                    ShowPopup("You should try speaking louder");
                    popupActive = true;
                    ifPopupShownAlready[0] = true;
                }
                else if (lookTimers.LonglookAway && !ifPopupShownAlready[1])
                {
                    ShowPopup("You should try to look at the audience often while presenting");
                    popupActive = true;
                    ifPopupShownAlready[1] = true;
                }
                else if (wordCount.wpm != 0)
                {
                    if (wordCount.wpm < 25 && !ifPopupShownAlready[2])
                    {
                        ShowPopup("You should try speaking faster");
                        popupActive = true;
                        ifPopupShownAlready[2] = true;
                    }
                    else if (wordCount.wpm > 80 && !ifPopupShownAlready[3])
                    {
                        ShowPopup("You should try speaking slower");
                        popupActive = true;
                        ifPopupShownAlready[3] = true;
                    }
                }
            }
            else
            {
                if (count == popupTimeCounter)
                {
                    HidePopup();
                    popupActive = false;
                    count = 0;
                    wait = 0;
                }
                else { count++; }
            }
        }
        else
        {
            wait++;
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
