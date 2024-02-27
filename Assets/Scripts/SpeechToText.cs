using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenAI;
using Samples.Whisper;
public class SpeechToText : MonoBehaviour
{
    private AudioClip clip;
    private byte[] bytes;
    int maxtime = 5;
    bool recording;
    private readonly string fileName = "output.wav";
    private OpenAIApi openai = new OpenAIApi("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");
    // Start is called before the first frame update
    void Start()
    {
        recording = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)&&!recording)
        {
            Debug.Log("Started");
            StartRecording();
        }
        else if (recording && (Microphone.GetPosition(null) >= clip.samples || Input.GetKeyDown(KeyCode.Tab)))
        {
            Debug.Log("Stopped");
            StopRecording();
        }

    }

    private void StartRecording()
    {
        clip = Microphone.Start(null, false, maxtime, 44100);
        recording = true;
    }

    private async void StopRecording()
    {
        Microphone.End(null);
        recording = false;

        byte[] data = SaveWav.Save(fileName, clip);

        var req = new CreateAudioTranscriptionsRequest
        {
            FileData = new FileData() { Data = data, Name = "audio.wav" },
            // File = Application.persistentDataPath + "/" + fileName,
            Model = "whisper-1",
            Language = "lt"
        };
        var res = await openai.CreateAudioTranscription(req);

        Debug.Log(res.Text.ToString());

    }
}