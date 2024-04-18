using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenAI;
using TMPro;
using System;
using UnityEngine.UI;


public class SpeechToText : MonoBehaviour
{
    private AudioClip clip;
    private byte[] bytes;
    bool recording;
    string api_key;
    private readonly string fileName = "output.wav";
    private OpenAIApi openai;
    private float recordDuration = 5f; // Duration to record in seconds
    private float waitTime = 10f; // Time between recordings (10 for demo, 30 for production)
    private float recordTimer = 0f;
    private List<string> responses;
    private int numberOfResponses;
    public double wpm;

    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private TextMeshProUGUI textBoxWPM;
    private XRIDefaultInputActions controls;

    [SerializeField] private Button startStopButton;
    [SerializeField] private Text buttonText;


    // Start is called before the first frame update
    private void Awake()
    {
        api_key = File.ReadAllText("api_key.txt");
        openai = new OpenAIApi(api_key);
        controls = new XRIDefaultInputActions();
    }
    void Start()
    {
        responses = new List<string>();
        recording = false;
        recordTimer = Time.time + 5f; //Time until first recording(5 for demo, 10-15 for production)

        if(startStopButton != null)
            startStopButton.onClick.AddListener(() => ButtonAction());
    }

    // Update is called once per frame
    void Update()
    {
        //Recording makes unity freeze for a second. Turned it off for demonstration
        //if (Time.time >= recordTimer)
        //{
        //    Debug.Log("Started");
        //    StartRecording();
        //    recordTimer = Time.time + recordDuration + waitTime;
        //}
        //if (recording && Time.time >= recordTimer - recordDuration)
        //{
        //    Debug.Log("Stopped");
        //    StopRecording();
        //}

        if (Input.GetKeyDown(KeyCode.Tab)&&!recording)
        {
            Debug.Log("Started");
            StartRecording();
        }
        else if (recording && (Microphone.GetPosition(null) >= clip.samples || Input.GetKeyDown(KeyCode.Tab)))
        {
            Debug.Log("Stopped");
            StopRecording();
        }

        if (controls.XRILeftHand.Transcript.WasPerformedThisFrame() && !recording)
        {
            Debug.Log("Started");
            textBox.text = "Klausoma...";
            StartRecording();
        }
        else if (recording && (Microphone.GetPosition(null) >= clip.samples || controls.XRILeftHand.Transcript.WasPerformedThisFrame()))
        {
            Debug.Log("Stopped");
            textBox.text = "Generuojama...";
            StopRecording();
        }

    }

    private void ButtonAction()
    {
        if (!recording)
        {
            Debug.Log("Started");
            textBox.text = "Klausoma...";
            buttonText.text = "Sustabdyti kalba";
            StartRecording();
        }
        else if (recording)
        {
            Debug.Log("Stopped");
            textBox.text = "Generuojama...";
            buttonText.text = "Pradeti kalba";
            StopRecording();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void StartRecording()
    {
        if (AudioManager.instance.micDeviceName != "")
            clip = Microphone.Start(AudioManager.instance.micDeviceName, false, Convert.ToInt32(recordDuration), 44100);
        else
            clip = Microphone.Start(null, false, Convert.ToInt32(recordDuration), 44100);

        recording = true;
    }

    private async void StopRecording()
    {
        Microphone.End(null);
        recording = false;

        byte[] data = Samples.Whisper.SaveWav.Save(fileName, clip);

        var req = new CreateAudioTranscriptionsRequest
        {
            FileData = new FileData() { Data = data, Name = "audio.wav" },
            // File = Application.persistentDataPath + "/" + fileName,
            Model = "whisper-1",
            Language = "lt"
        };
        var res = await openai.CreateAudioTranscription(req);
        
        Debug.Log(res.Text.ToString());
        if (res.Text.Length>1 && res.Text.Split(',').Length<30)
        {
            Debug.Log(res.Text.ToString());
            responses.Add(res.Text.ToString());
            numberOfResponses++;
            GetWPM();
            textBox.text = res.Text.ToString();
        }

        buttonText.text = "Pradeti kalba";
        GetComponent<Scorecalculation>().CalculateScore();
    }

    private void GetWPM()
    {
        double wordCount = 0;

        for(int i=0;i<responses.Count;i++)
        {
            wordCount += responses[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }

        wpm=  wordCount / Convert.ToDouble(numberOfResponses)*(60.0/recordDuration);
        textBoxWPM.text = "Wpm =" + wpm;
        Debug.Log("Wpm ="+wpm);
    }
}
