using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scorecalculation : MonoBehaviour
{
    [SerializeField ] private VolumeDetection volumeDetection;
    [SerializeField] private SpeechToText speechScript;
    [SerializeField] public Text TotalScoreTextui;
    [SerializeField] public Text VolumeScoreTextui;
    [SerializeField] public Text SpeechScoreTextui;
    private int idealWPMlow = 20; //ideal for presentation 100-150
    private int idealWPMhigh = 150;
    private double ratioForWPM = 0.15;
    private int idealVolumeLow = -45;
    private int idealVolumeHigh = -30;
    private double ratioForVolume = 0.15;
    private float score;
	private float vol_min = 0.0f;
    private float vol_max = 1.0f;
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
    public void CalculateScore()
    { 
        float speechScore = (float)(speechScript.wpm >= idealWPMlow && speechScript.wpm <= idealWPMhigh ?
            1 : speechScript.wpm < idealWPMlow ?
            1 - Mathf.Abs(115 - (float)speechScript.wpm) * ratioForWPM :
            1 - (idealWPMhigh - speechScript.wpm) * ratioForWPM);
        speechScore = speechScore > 0 ? speechScore : 0;
        Debug.Log("Speech score:"+ speechScore);
        SpeechScoreTextui.text = score.ToString();
        float volumeScore = (float)(volumeDetection.avarageVolume >= idealVolumeLow && volumeDetection.avarageVolume <= idealVolumeHigh ?
            1 : volumeDetection.avarageVolume < idealVolumeLow ?
            1 - (idealVolumeLow - volumeDetection.avarageVolume) * ratioForVolume :
            1 - (volumeDetection.avarageVolume - idealVolumeHigh) * ratioForVolume);

        volumeScore = Mathf.Clamp(volumeScore, vol_min, vol_max);
        Debug.Log("Volume score:" + volumeScore);
        VolumeScoreTextui.text = score.ToString();
        score = (float)(speechScore*0.8 + volumeScore*0.2);
		Debug.Log("Total score:" + score);
        TotalScoreTextui.text = score.ToString();
    }
}
