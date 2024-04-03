using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TTS : MonoBehaviour
{
    [SerializeField]  TTSManager ttsManager;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            if (ttsManager) ttsManager.SynthesizeAndPlay("Produkto vystymo projektas įveikti kalbėjimo baimę");
    }
}
