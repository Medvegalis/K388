using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    public Microphone getController;
    private AudioSource audioSource;   
    
    private Dropdown MicDropdown;
    private Dropdown AudioSourceDropdown;

    //here we store the detected devices
    List<string> getDeviceList = new List<string>();

    void Start()
    {
        MicDropdown = transform.GetComponent<Dropdown>();
        MicDropdown.ClearOptions();

        foreach (string device in Microphone.devices)
        {
            getDeviceList.Add(device);

        }

        MicDropdown.AddOptions(getDeviceList);


        MicDropdown.onValueChanged.AddListener(ChangeDefaultMic);
    }

    void ChangeDefaultMic(int value)
    { 
        AudioManager.instance.micDeviceName = Microphone.devices[value];
    }

}
