using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcEventHandler : MonoBehaviour
{
    public delegate void GenerateEvent(string message);
    public delegate void AskEvent();
    public static event GenerateEvent GenerationEvent;
    public static event AskEvent QuestionEvent;


    public static void GenerateQuestion(string snippet)
    {
        GenerationEvent(snippet);
    }
    public static void AskQuestion()
    {
        QuestionEvent();
    }
}
