using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioClip[] sounds;
    public float minTimeBetweenSounds = 5f;
    public float maxTimeBetweenSounds = 10f;

    private Dictionary<AudioSource, AudioClip> lastPlayedSounds = new Dictionary<AudioSource, AudioClip>();

    void Start()
    {
        // Start coroutine to play random sounds
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator<WaitForSeconds> PlayRandomSounds()
    {
        while (true)
        {
            // Wait for a random time before playing the next sound
            yield return new WaitForSeconds(Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds));

            // Choose a random AudioSource
            AudioSource selectedSource = audioSources[Random.Range(0, audioSources.Length)];

            // Choose a random sound that is not the last one played on the selected AudioSource
            AudioClip selectedClip = GetRandomClip(selectedSource);

            // Play the clip on the selected AudioSource
            selectedSource.clip = selectedClip;
            selectedSource.Play();

            // Update the dictionary with the last played sound for the selected source
            lastPlayedSounds[selectedSource] = selectedClip;
        }
    }

    AudioClip GetRandomClip(AudioSource source)
    {
        if (sounds.Length == 0)
        {
            Debug.LogWarning("No sounds assigned!");
            return null;
        }

        // Choose a random sound that is not the last one played on the selected AudioSource
        AudioClip randomClip;
        do
        {
            randomClip = sounds[Random.Range(0, sounds.Length)];
        }
        while (lastPlayedSounds.ContainsKey(source) && lastPlayedSounds[source] == randomClip);

        return randomClip;
    }
}
