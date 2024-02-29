using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        Debug.Log(lines.Length);
        Debug.Log(pageCount);
        Debug.Log(linesOnLastPage);
        currentPage = -1;
    }

    private void Update()
    {
        
        if(currentPage==-1  && Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentPage = 0;
            LoadPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPage>0)
        {
            currentPage--;
            LoadPage();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)&& currentPage<pageCount-1)
        {
            currentPage++;
            LoadPage();
        }
    }


    private void LoadPage()
    {
        clipboardText.text = "";
        if (currentPage == pageCount-1)
        {
            for(int i=0; i<linesOnLastPage;i++)
            {
                clipboardText.text += '\n' + pages[currentPage, i];
            }
        }
        else
        {
            for (int i = 0; i < maxLines; i++)
            {
                Debug.Log(currentPage);
                clipboardText.text += '\n' + pages[currentPage, i];
            }
        }
    }
    
    //TODO Implement that character ';' would act as a page break
    private void SplitToPages()
    {
        pageCount = lines.Length / maxLines + 1;
        for (int i = 0; i < pageCount; i++)
        {
            int offset = maxLines * i;
            //jei >8 eilutes
            if (lines.Length > maxLines * (i + 1))
            {
               for(int k=0; k<maxLines;k++)
                {
                    pages[i,k] = lines[offset+k];
                }
                     
            }
            //jei <8 eilutes(last iteration)
            else
            {
                for(int k=0;k<lines.Length-i*8;k++)
                {
                    pages[i, k] = lines[offset+k];
                    linesOnLastPage++;
                }
            }
        }
    }
    private void SplitLongLines()
    {
        List<string> modifiedLines = new List<string>();

        foreach (string line in lines)
        {
            if (line.Length > maxCharactersInLine)
            {
                int splitIndex = maxCharactersInLine;
                string firstLine = line.Substring(0, splitIndex);
                string secondLine = line.Substring(splitIndex);
                modifiedLines.Add(firstLine);
                modifiedLines.Add(secondLine);
            }
            else
            {
                modifiedLines.Add(line);
            }
        }

        lines = modifiedLines.ToArray();
    }
}
