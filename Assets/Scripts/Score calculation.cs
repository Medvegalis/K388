using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Scorecalculation : MonoBehaviour
{
    [SerializeField ] private VolumeDetection volumeDetection;
    [SerializeField] private SpeechToText speechScript;
    [SerializeField] public TextMeshProUGUI textui;
    private int idealWPMlow = 110; //ideal for presentation 100-150
    private int idealWPMhigh = 150;
    private double ratioForWPM = 0.15;
    private int idealVolumeLow = -45;
    private int idealVolumeHigh = -30;
    private double ratioForVolume = 0.15;
    private float score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO change this to speech end event
        if (Input.GetKeyDown(KeyCode.T))
            CalculateScore();
    }
    void CalculateScore()
    {
        float speechScore = (float)(speechScript.wpm >= idealWPMlow && speechScript.wpm <= idealWPMhigh ? 1 : speechScript.wpm < idealWPMlow ? 1 - Mathf.Abs(115 - (float)speechScript.wpm) * ratioForWPM : 1 - (idealWPMhigh - speechScript.wpm) * ratioForWPM);
        speechScore = speechScore > 0 ? speechScore : 0;
        Debug.Log("Speech score:"+ speechScore);
        float volumeScore = (float)(volumeDetection.avarageVolume >= idealVolumeLow && volumeDetection.avarageVolume <= idealVolumeHigh ? 1 : volumeDetection.avarageVolume < idealVolumeLow ? 1 - (idealVolumeLow - volumeDetection.avarageVolume) * ratioForVolume : 1 - (volumeDetection.avarageVolume - idealVolumeHigh) * ratioForVolume);
        volumeScore = volumeScore > 0 ? volumeScore : 0;
        Debug.Log("Volume score:" + speechScore);
        score = (speechScore + volumeScore) / 2;
        Debug.Log("Total score:" + score);
        textui.text = score.ToString();
    }
}
