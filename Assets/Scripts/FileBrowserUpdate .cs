
using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
public class FileBrowserUpdate : MonoBehaviour
{
    public string format;
    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = string.Format("Image files (*.{0}) | *.{1};",format,format);
        bp.title = format=="txt"? "Select your notes":"Select your slides";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(LoadFile(path));
        });
    }

    IEnumerator LoadFile(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(path))
        {
            yield return uwr.SendWebRequest();

#pragma warning disable CS0618 // Type or member is obsolete
            if (uwr.isNetworkError || uwr.isHttpError)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                Debug.Log(uwr.error);
            }
            else
            {
                string filename = format == "txt" ? "speech" : "slides";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),"Assets" ,"Resources",string.Format("{0}.{1}",filename,format));
                File.WriteAllBytes(filePath, uwr.downloadHandler.data);
                //Debug.Log("File saved to: " + filePath);
            }
        }
    }
}
