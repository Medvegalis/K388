using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] private AudioMixer Master;
    [SerializeField] private Slider masterSlider;

    public string micDeviceName = null;

    private void Awake()
    {
        instance = this;
        masterSlider.onValueChanged.AddListener(setMasterVolume);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMasterVolume(float volume)
    {
        Master.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
}
