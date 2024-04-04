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
    public int micIndex = 0;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

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
