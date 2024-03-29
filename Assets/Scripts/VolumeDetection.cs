using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        string microphoneName = devices[microphoneNR];

        // Start recording from the selected microphone
        audioSource.clip = Microphone.Start(microphoneName, true, 1, sampleRate);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(microphoneName) > 0)) { } // Wait until the recording has started
        audioSource.Play();

        // Initialize array for storing RMS values
        int numSamples = Mathf.RoundToInt(sampleRate * updateInterval);
        rmsValues = new float[numSamples];
    }

    void Update()
    {
        // Calculate volume level (RMS) of the captured audio
        float[] waveData = new float[512]; // Adjust the size as needed
        string microphoneName = Microphone.devices[microphoneNR];
        int micPosition = Microphone.GetPosition(microphoneName) - (waveData.Length + 1);
        if (micPosition < 0)
            return;

        audioSource.clip.GetData(waveData, micPosition);

        // Calculate RMS (root mean square) to get volume
        float rms = 0;
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
            float averageRms = 0;
            foreach (float value in rmsValues)
            {
                averageRms += value;
            }
            averageRms /= rmsValues.Length;

            // Display the average RMS
            Debug.Log("Average speech volume over " + updateInterval + " seconds: " + averageRms);
            currentIndex = 0;
            // Reset time since last update
            timeSinceLastUpdate = 0;
        }
    }

    void OnDisable()
    {
        // Stop microphone when the object is disabled
        string microphoneName = Microphone.devices[microphoneNR];
        Microphone.End(microphoneName);
    }
}