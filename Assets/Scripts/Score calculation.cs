using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scorecalculation : MonoBehaviour
{
	[SerializeField] private VolumeDetection volumeDetection;
	[SerializeField] private SpeechToText speechScript;
	[SerializeField] public Text TotalScoreTextui;
	[SerializeField] public Text VolumeScoreTextui;
	[SerializeField] public Text SpeechScoreTextui;
    [SerializeField] public Text DistanceScoreTextui;
    [SerializeField] private LookTimers lookTimers;
	
	private int idealWPMlow = 40;
	private int idealWPMhigh = 100;
	private double ratioForWPM = 0.15;
	private int idealVolumeLow = 30;
	private int idealVolumeHigh = 80;
	private double ratioForVolume = 0.15;
	private float score;
	private float prev_dist = 0.0f;
	private float vol_min = 0.0f;
	private float vol_max = 10.0f;
	private float avg_vol = 60.0f;
	private float dist_min = 0.01f;
	// Start is called before the first frame update
	void Start()
	{
		if(lookTimers == null)
		{
			lookTimers = GameObject.FindObjectOfType<LookTimers>();
		}
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
		Debug.Log("WPM: " + speechScript.wpm);
		float vol_abs = Mathf.Abs(volumeDetection.averageVolume);
		Debug.Log("Abs_Volume: " + vol_abs);
		float temp = lookTimers.DistanceSum();
		float dist = Mathf.Abs(prev_dist - temp);
		prev_dist = temp;
		

		float coef1 = (float)5 / (idealWPMhigh - idealWPMlow);
		float speechScore = (float)(speechScript.wpm >= idealWPMlow ?
			(5 + 5 / (1 + 200 * Mathf.Exp((float)(((-1) * coef1) * speechScript.wpm)))) :
			Mathf.Exp((float)(speechScript.wpm / 21.172)) - 1);
		Debug.Log("Speech score: " + speechScore);
		SpeechScoreTextui.text = speechScore.ToString();

		float coef2 = (float)5 / Mathf.Abs(idealVolumeHigh - idealVolumeLow);
		float coef3 = (float) (1 + 200 * Mathf.Exp((float)(((-1) * coef2) * vol_abs)));
		float volumeScore = (float)(vol_abs >= idealVolumeLow ?
			5 + (float) (5 / coef3) :
			Mathf.Exp((float)(vol_abs / (5+ vol_abs))) - 1);
		//volumeScore = Mathf.Clamp(volumeScore, vol_min, vol_max);
		Debug.Log("Volume score: " + volumeScore);
		VolumeScoreTextui.text = volumeScore.ToString();

		float distScore = (float)(dist >= dist_min ?
			(float) dist/(dist+10)*10 : 0);
		Debug.Log("Distance score: " + distScore);
		DistanceScoreTextui.text = distScore.ToString();

		score = (float)(speechScore * 0.6 + volumeScore * 0.3 + distScore * 0.1);
		Debug.Log("Total score: " + score);
		TotalScoreTextui.text = score.ToString();
	}

}
