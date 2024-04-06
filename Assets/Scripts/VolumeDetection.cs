using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class VolumeDetection : MonoBehaviour
{
    // Adjustable parameters
    public int sampleRate = 44100;
    public float normalizationFactor = 0.1f; // Adjust this value to normalize RMS
    public float updateInterval = 0.7f; // Time interval to update and display RMS
    public int microphoneNR = 0;
    private AudioSource audioSource;
    private float[] rmsValues;
    private int currentIndex = 0;
    private float timeSinceLastUpdate = 0;
    private int micPosition;
    float[] waveData;
    string microphoneName;
    float rms;
    float averageRms;

    public float avarageVolume;
    private int sampleCount;
    private List<double> samples;

    public TextMeshProUGUI textUi;

    void Start()
    {
        // Create a new audio source for capturing microphone input
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.mute = true;
        // Get the names of available microphones
        string[] devices = Microphone.devices;

        // Check if any microphone is available
        if (devices.Length == 0)
        {
            Debug.LogError("No microphone found!");
            return;
        }

        // Use the first available microphone
        microphoneName = devices[microphoneNR];

        // Start recording from the selected microphone
        audioSource.clip = Microphone.Start(microphoneName, true, 1, sampleRate);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(microphoneName) > 0)) { } // Wait until the recording has started
        audioSource.Play();

        // Initialize array for storing RMS values
        int numSamples = Mathf.RoundToInt(sampleRate * updateInterval);
        rmsValues = new float[numSamples];
        waveData = new float[512]; // Adjust the size as needed
    }

    void Update()
    {
        // Calculate volume level (RMS) of the captured audio
        micPosition = Microphone.GetPosition(microphoneName) - (waveData.Length + 1);
        if (micPosition < 0)
            return;

        audioSource.clip.GetData(waveData, micPosition);

        // Calculate RMS (root mean square) to get volume
         rms = 0;
        foreach (float sample in waveData)
        {
            rms += sample * sample;
        }
        rms = Mathf.Sqrt(rms / waveData.Length);

        // Normalize RMS value
        rms *= normalizationFactor;

        // Store the RMS value
        rmsValues[currentIndex] = rms;
        currentIndex = (currentIndex + 1) % rmsValues.Length;

        // Update time since last display
        timeSinceLastUpdate += Time.deltaTime;

        // Check if it's time to update and display RMS
        if (timeSinceLastUpdate >= updateInterval)
        {
            // Calculate average RMS over the update interval
             averageRms = 0;
            foreach (float value in rmsValues)
            {
                averageRms += value;
            }
            averageRms /= rmsValues.Length;
            double averageDb = Math.Round(10 * Mathf.Log10(averageRms), 0);
            // Display the average RMS in db
            //Debug.Log("Average speech volume over " + updateInterval + " seconds: " + averageDb);
            
            textUi.text = averageDb.ToString();
            currentIndex = 0;
            // Reset time since last update
            timeSinceLastUpdate = 0;
            samples.Add(averageDb);
            GetAvarageDB();
        }
    }

    void OnDisable()
    {
        // Stop microphone when the object is disabled
        string microphoneName = Microphone.devices[microphoneNR];
        Microphone.End(microphoneName);
    }

    void GetAvarageDB()
    {
        double volume = 0;

        for (int i = 0; i < samples.Count; i++)
        {
            volume += samples[i];
        }

        avarageVolume = (float)(volume / Convert.ToDouble(sampleCount));

    }
}