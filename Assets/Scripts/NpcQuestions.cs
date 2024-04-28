using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using System.IO;
public class NpcQuestions : MonoBehaviour
{
    public SpeechToText stt;
    private OpenAIApi openai;
    string api_key;
    private XRIDefaultInputActions controls;
    [SerializeField] GameObject npcs;
    [SerializeField] TTSManager ttsManager;
    private List<string> questions;
    private int AskedQuestions;
    private void Awake()
    {
        questions = new List<string>();
        api_key = File.ReadAllText("api_key.txt");
        openai = new OpenAIApi(api_key);
        controls = new XRIDefaultInputActions();
        NpcEventHandler.QuestionEvent += AskQuestion;
        NpcEventHandler.GenerationEvent += GenerateQuestions;
        AskedQuestions = 0;

    }

    void AskQuestion()
    {
        if (AskedQuestions < questions.Count)
        {
            if (ttsManager) ttsManager.SynthesizeAndPlay(questions[AskedQuestions]);
            AskedQuestions++;
        }
    }
    async void GenerateQuestions(string snippet)
    {
            List<ChatMessage> messages = new List<ChatMessage>();
            messages.Add(new ChatMessage { Role = "system", Content = "You are a helpful assistant" });
            messages.Add(new ChatMessage { Role = "user", Content =snippet });
            messages.Add(new ChatMessage { Role = "assistant", Content = "This is a snippet from a presentation that has ended, ask a question for this presentation.Question should be in a language the snippet is provided" });
            var req = new CreateChatCompletionRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = messages

            };
            var res = await openai.CreateChatCompletion(req);
            questions.Add(res.Choices[0].Message.Content);
        
    }
}
