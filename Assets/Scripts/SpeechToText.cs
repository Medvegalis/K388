using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenAI;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;


public class SpeechToText : MonoBehaviour
{
    private AudioClip clip;
    bool recording;
    string api_key;
    private readonly string fileName = "output.wav";
    private OpenAIApi openai;
    private float recordDuration = 60f;
    private List<string> responses;
    private int numberOfResponses;
    public double wpm;
    public int fillerWC;

	[SerializeField] private LookTimers lookTimers;
	[SerializeField] private Text textBox;
    [SerializeField] private Text textBoxWPM;
    private XRIDefaultInputActions controls;

    [SerializeField] private Button startStopButton;
    [SerializeField] private Text buttonText;

    [SerializeField] private Button startQuestionsButton;

    [SerializeField] private Animator maleTwoAnimator;
    [SerializeField] private Animator maleOneAnimator;
    [SerializeField] private Animator femaleOneAnimator;
    [SerializeField] private Animator femaleTwoAnimator;
    [SerializeField] private Animator femaleThreeAnimator;
    // Start is called before the first frame update
    private void Awake()
    {
        api_key = File.ReadAllText("api_key.txt");
        openai = new OpenAIApi(api_key);
        controls = new XRIDefaultInputActions();
    }
    void Start()
    {
        fillerWC = 0;
        responses = new List<string>();
        recording = false;

        if(startStopButton != null)
            startStopButton.onClick.AddListener(() => ButtonAction());


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab)&&!recording)
        {
            Debug.Log("Started");
            textBox.text = "Klausoma...";
            buttonText.text = "Sustabdyti kalba";
            StartRecording();
        }
        else if (recording && (Microphone.GetPosition(null) >= clip.samples || Input.GetKeyDown(KeyCode.Tab)))
        {
            Debug.Log("Stopped");
            textBox.text = "Generuojama...";
            buttonText.text = "Pradeti kalba";
            StopRecording();
        }

        if (controls.XRILeftHand.Transcript.WasPerformedThisFrame() && !recording)
        {
            Debug.Log("Started");
            textBox.text = "Klausoma...";
            buttonText.text = "Sustabdyti kalba";
            StartRecording();
        }
        else if (recording && (Microphone.GetPosition(null) >= clip.samples || controls.XRILeftHand.Transcript.WasPerformedThisFrame()))
        {
            Debug.Log("Stopped");
            textBox.text = "Generuojama...";
            buttonText.text = "Pradeti kalba";
            StopRecording();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            NpcEventHandler.AskQuestion();

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
        StartCoroutine(npcAnimationStart());
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
		textBox.text = res.Text.ToString();
        CountFillerWords(res.Text.ToString());

        if (res.Text.Length>1 && res.Text.Split(',').Length<1000)
        {
            NpcEventHandler.GenerateQuestion(res.Text);
            Debug.Log(res.Text.ToString());
            responses.Add(res.Text.ToString());
            numberOfResponses++;
            GetWPM();
        }

        buttonText.text = "Pradeti kalba";
        GetComponent<Scorecalculation>().CalculateScore();
        lookTimers.DistanceSum = 0;
        if(startQuestionsButton != null)
            startQuestionsButton.interactable = true;

        maleTwoAnimator.SetTrigger("SpeechFinished");
        maleOneAnimator.SetTrigger("SpeechFinished");
        femaleThreeAnimator.SetTrigger("ThumbsUp");
        femaleOneAnimator.SetTrigger("SpeechFinished");

    }

    private void GetWPM()
    {
        double wordCount = 0;

        for(int i=0;i<responses.Count;i++)
        {
            wordCount += responses[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }

        wpm=  wordCount / Convert.ToDouble(numberOfResponses)*(60.0/recordDuration);
        textBoxWPM.text = wpm.ToString();
        Debug.Log("Wpm ="+wpm);
    }
    private void CountFillerWords(string input)
    {
        string pattern = @"\b(u+m+|h+m+|a+h+|eh|er|huh|m+hm+)\b";

        MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        fillerWC += matches.Count;
    }
    private IEnumerator npcAnimationStart()
    {
        yield return new WaitForSeconds(5f);
        femaleOneAnimator.SetTrigger("StartTalking");
        maleTwoAnimator.SetTrigger("StartTalking");
        femaleTwoAnimator.SetTrigger("SitPositionChange");
    }

}
