using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static FirebaseDatabaseManager;

public class Point
{
	public double Heartrate { get; set; }
	public double Timepoint { get; set; }
	public string Classif { get; set; }
	public Point(double heartrate, double time)
	{
		Heartrate = heartrate;
		Timepoint = time;
	}
}

public class FirebaseDatabaseManager : MonoBehaviour
{
	DatabaseReference reference;
	int index = 0;
	List<Point> points = new List<Point>();
	Point c1 = new Point(50.0, 1.0);
	Point c2 = new Point(60.0, 2.0);
	[SerializeField] private Text uiText;
	void Start()
	{
		reference = FirebaseDatabase.DefaultInstance.RootReference;

	}
	private void Update()
	{
		ReadDatabase();
	}

	static public double Dist_Point(Point p1, Point p2)
	{
		double t1 = p1.Timepoint - p2.Timepoint;
		double h1 = p1.Heartrate - p2.Heartrate;
		return Math.Sqrt(t1*t1 + h1*h1);
	}

	static public void Classification(Point c1, Point c2, List<Point> pts)
	{
		foreach(var pt in pts)
		{
			double d1 = Dist_Point(pt, c1);
			double d2 = Dist_Point(pt, c2);
			if(d1<d2)
				pt.Classif = "0";
			else
				pt.Classif = "1";
		}
	}

	public static void Change_Centre(List<Point> points, Point c, string cls)
	{
		var cpts = points.Where(p => p.Classif == cls);
		if (cpts.Count() > 0)
		{
			double avg_hr = cpts.Average(p => p.Heartrate);
			double avg_time = cpts.Average(p => p.Timepoint);
			c.Heartrate = avg_hr;
			c.Timepoint = avg_time;
		}
	}

	public static string Detect_Class(Point c1, Point c2, Point last_pt)
	{
		double maxhr = Math.Max(c1.Heartrate, c2.Heartrate);
		double minhr = Math.Min(c1.Heartrate, c2.Heartrate);
		double temp1 = Math.Abs(minhr - last_pt.Heartrate);
		double temp2 = Math.Abs(maxhr - last_pt.Heartrate);
		if (temp1 < temp2)
			return "Normalus";
		else if (temp1 > temp2)
			return "Stresuojantis";
		else
			return "<Renkame_duomenis>";
	}

	public void ReadDatabase()
	{
		reference.Child("POLOHPPI")
		 .GetValueAsync().ContinueWithOnMainThread(task =>
		 {
			 if (task.IsFaulted)
			 {
				 Debug.Log(task.Exception.Message);
			 }
			 else if (task.IsCompleted)
			 {
				 DataSnapshot snapshot = task.Result;
				 if (snapshot != null && snapshot.Exists)
				 {
					 if (double.TryParse(snapshot.Value.ToString(), out double value))
					 {
						 double HR = 60 / ((double)value / 1000);
						 Point last_point = new Point(HR, index++);
						 points.Add(last_point);
						 for (int r = 0; r < 3; r++)
						 {
							 Classification(c1,c2,points);
							 Change_Centre(points, c1, "0");
							 Change_Centre(points, c2, "1");
						 }
						 string final_class = Detect_Class(c1, c2, last_point);
						 uiText.text = final_class;

					 }
				 }
			 }
		 });
	}


}