using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEngine.UI;
using System.IO;
using Shubham.PDF;

//using Image = System.Drawing;
//C:\Users\shubh\Downloads\Docs\HKUST_EDX_Certificate.pdf

public class PdfReader : MonoBehaviour {
	
	public int totalPageCnt = 0;
	public UnityEngine.UI.RawImage pdfSceneImg;

	private int currPageIndex = 0;
	private PdfToImage pti;
    //private List<Texture2D> pdfPages = new List<Texture2D> ();

    //C:\Users\shubh\Documents\Unity_Projects\OpenCVTest\Assets\StreamingAssets\cert.pdf
    public void Start()
    {
		InitialisePdf(Directory.GetCurrentDirectory()+@"\Assets\Resources\slides.pdf");
    }

    public void Update()
    {
		if (Input.GetKeyDown(KeyCode.RightArrow))
			ShowNextPage();
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			ShowPrevPage();
    }


    public void InitialisePdf(string path)
	{
		//pdfPages.Clear ();
		totalPageCnt = 0;
		//Debug.Log(path.text);
		pti = new PdfToImage ();
		PdfToImage.logs += PdfLog;

				Debug.Log (pti.Test());
		Debug.Log(path);
		if (pti.ReadPdf (path, ref totalPageCnt))
			 Debug.Log("Page: (" + (currPageIndex + 1).ToString () + "/" + totalPageCnt.ToString () + ")");
		else
			Debug.Log("unable to read");
		
		pdfSceneImg.texture = pti.GetDrawImageFromPdf(0);
		//pdfSceneImg.texture = pdfPages [0];
	}
	/*
	public Texture2D ConvertImageToTexture(System.Drawing.Image image)
	{
		Texture2D tex = new Texture2D (image.Width, image.Height);
		MemoryStream ms = new MemoryStream ();
		image.Save (ms, System.Drawing.Imaging.ImageFormat.Jpeg);
		
		ms.Seek (0, SeekOrigin.Begin);
		tex.LoadImage (ms.ToArray ());
		
		ms.Close ();
		ms = null;

		return tex;
	}*/

	public void ShowNextPage()
	{
		if (currPageIndex >= totalPageCnt-1)
			return;

		currPageIndex++;
		
		pdfSceneImg.texture = pti.GetDrawImageFromPdf(currPageIndex);
		Debug.Log("Page: (" + (currPageIndex + 1).ToString() + "/" + totalPageCnt.ToString() + ")");
	}

	public void ShowPrevPage()
	{
		if (currPageIndex < 1)
			return;

		currPageIndex--;

		pdfSceneImg.texture = pti.GetDrawImageFromPdf(currPageIndex);
		Debug.Log("Page: (" + (currPageIndex + 1).ToString() + "/" + totalPageCnt.ToString() + ")");
	}

	public void PdfLog(string msg)
	{
		Debug.Log ("[PDF]: " + msg);
	}
}
