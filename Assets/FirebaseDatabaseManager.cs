using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FirebaseDatabaseManager;

public class Point
{
	private double heartrate;
	private double time;
	private string classif;
	public Point(double heartrate, double time)
	{
		this.heartrate = heartrate;
		this.time = time;
	}
	public string Classif
	{
		get { return classif; }
		set { classif = value; }
	}
	public double Time { get { return time; } }
	public double HeartRate { get { return heartrate; } }
}

public class FirebaseDatabaseManager : MonoBehaviour
{
	string userId;
	DatabaseReference reference;
	int index = 0;
	List<Point> points = new List<Point>();
	int algor_iter = 50;
	Point c1 = new Point(50.0, 1.0);
	Point c2 = new Point(60.0, 2.0);
	void Start()
	{
		userId = SystemInfo.deviceUniqueIdentifier;
		reference = FirebaseDatabase.DefaultInstance.RootReference;

	}
	private void Update()
	{
		ReadDatabase();
	}

	static public void Print_Array(List<Point> pts, int index)
	{

		string temp = "";
		temp += $"[";
		for (int i = 0; i < index; i++)
		{
			Point pt = pts[i];
			temp += $"({pt.HeartRate};{pt.Time};{pt.Classif})\n";
		}
		temp += $"]";
	}

	static public double Dist_Point(Point p1, Point p2)
	{
		return Math.Sqrt(Math.Pow(p1.Time - p2.Time, 2) + Math.Pow(p1.HeartRate - p2.HeartRate, 2));
	}

	static public void Simple_Algo(Point c1, Point c2, List<Point> pts, int index)
	{
		for (int i = 0; i <= index; i++)
		{
			Point pt = pts[i];
			double d1 = Dist_Point(pt, c1);
			double d2 = Dist_Point(pt, c2);
			if (d1 < d2)
				pt.Classif = "0";
			else pt.Classif = "1";
			pts[i] = pt;
		}

	}

	public static void Change_Centre(List<Point> points, Point c, string cls, ref double temp_hr, ref double temp_time)
	{
		var cpts = points.Where(p => p.Classif == cls);
		if (cpts.Count() > 0)
		{
			foreach (var pt in cpts)
			{
				temp_hr += pt.HeartRate;
				temp_time += pt.Time;
			}
			temp_hr = (double)temp_hr / cpts.Count();
			temp_time = (double)temp_time / cpts.Count();
		}
		else
		{
			temp_hr = c.HeartRate;
			temp_time = c.Time;
		}
	}

	public static string Detect_Class(Point c1, Point c2, Point lpt)
	{
		double temp1 = Math.Abs(c1.HeartRate - lpt.HeartRate);
		double temp2 = Math.Abs(c2.HeartRate - lpt.HeartRate);
		if (temp1 < temp2)
			return "Normal";
		else if (temp1 > temp2)
			return "Stressed";
		return "Not_Detected";
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
						 Point point1 = new Point(HR, index);
						 points.Add(point1);
						 Simple_Algo(c1, c2, points, index);
						 index++;
						 double temp_hr = 0;
						 double temp_time = 0;
						 Change_Centre(points, c1, "0", ref temp_hr, ref temp_time);
						 c1 = new Point(temp_hr, temp_time);
						 temp_hr = 0;
						 temp_time = 0;
						 Change_Centre(points, c2, "1", ref temp_hr, ref temp_time);
						 c2 = new Point(temp_hr, temp_time);
						 //Debug.Log($"c1=({Math.Round(c1.HeartRate, 2)};{c1.Time}); c2=({Math.Round(c2.HeartRate, 2)};{c2.Time})");
						 Point lpt = points[points.Count() - 1];
						 //Debug.Log($"Added_point: {Math.Round(lpt.HeartRate,2)};{lpt.Time};{lpt.Classif}");
						 Debug.Log($"Speech_Condition: {Detect_Class(c1,c2,lpt)}");
					 }
				 }
			 }
		 });
	}


}