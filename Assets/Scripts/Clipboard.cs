using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Net.Sockets;

public class Clipboard : MonoBehaviour
{
    private string[] lines;
    public TMP_Text clipboardText;
    private static int maxPages = 20;
    private static int maxLines = 8;
    private static int maxCharactersInLine = 13;
    private int pageCount;
    private int linesOnLastPage;
    private string[,] pages;
    private int currentPage;

    void Start()
    {
        pages = new string[maxPages, maxLines];
        TextAsset bindata = Resources.Load("speech") as TextAsset;
        string text = bindata.ToString();
        lines = text.Split('\n');
        SplitLongLines();
        SplitToPages();
        currentPage = -1;
    }

    private void Update()
    {

        if (InputManager.instance.playerControls.Vr.Pirmary.WasPerformedThisFrame())
        {
            Debug.Log("Pressed primary");
        }
        VrControls();
        //PcControls();
    }

    private void VrControls()
    {
        if (InputManager.instance.currentRightHeldObjName == "InteractableCliboard")
        {
            Debug.Log("Holding clipboard");
            if (currentPage == -1 && InputManager.instance.playerControls.Vr.Pirmary.WasPerformedThisFrame())
            {
                currentPage = 0;
                LoadPage();
            }
            else if (InputManager.instance.playerControls.Vr.Secondary.WasPerformedThisFrame() && currentPage > 0)
            {
                currentPage--;
                LoadPage();
            }
            else if (InputManager.instance.playerControls.Vr.Pirmary.WasPerformedThisFrame() && currentPage < pageCount - 1)
            {
                currentPage++;
                LoadPage();
            }
        }
    }

    private void PcControls()
    {
        if (currentPage == -1 && Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentPage = 0;
            LoadPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPage > 0)
        {
            currentPage--;
            LoadPage();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentPage < pageCount - 1)
        {
            currentPage++;
            LoadPage();
        }
    }


    private void LoadPage()
    {
        clipboardText.text = "";
        if (currentPage == pageCount - 1)
        {
            for (int i = 0; i < linesOnLastPage; i++)
            {
                clipboardText.text += '\n' + pages[currentPage, i];
            }
        }
        else
        {
            for (int i = 0; i < maxLines; i++)
            {
                clipboardText.text += '\n' + pages[currentPage, i];
            }
        }
    }
    private void SplitToPages()
    {
        pageCount = lines.Length / maxLines + 1;
        int offset = 0;
        for (int i = 0; i < pageCount; i++)
        {
            //jei >8 eilutes
            if (lines.Length > offset + 8)
            {
                for (int k = 0; k < maxLines; k++)
                {
                    if (lines[offset].Length > 0 && lines[offset][0] == ';')
                    {
                        pageCount++;
                        offset++;
                        break;
                    }
                    else
                    {
                        pages[i, k] = lines[offset];
                        offset++;
                    }
                }

            }
            //jei <8 eilutes
            else
            {
                for (int k = 0; k < lines.Length - offset; k++)
                {
                    if (lines[offset][0] == ';')
                    {
                        linesOnLastPage = 0;
                        pageCount++;
                        offset++;
                        break;
                    }
                    else
                    {
                        pages[i, k] = lines[offset];
                        linesOnLastPage++;
                        offset++;
                    }
                }
            }
        }
    }

    private void SplitLongLines()
    {
        List<string> modifiedLines = new();
        foreach (string line in lines)
        {
            if (line.Length > maxCharactersInLine && line[maxCharactersInLine] != '\r')
            {
                bool done = false;
                string temp = line;
                while (!done)
                {
                    int splitIndex = maxCharactersInLine;
                    string firstLine = temp.Substring(0, splitIndex);
                    modifiedLines.Add(firstLine);
                    string theRest = temp.Substring(splitIndex);
                    if (theRest.Length > maxCharactersInLine && line[maxCharactersInLine] != '\r')
                    {
                        temp = theRest;
                    }
                    else
                    {
                        modifiedLines.Add(theRest);
                        done = true;
                    }
                }
            }
            else
            {
                modifiedLines.Add(line);
            }
        }

        lines = modifiedLines.ToArray();
    }
}
