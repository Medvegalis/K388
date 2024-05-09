using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseDatabaseManager : MonoBehaviour
{
	string userId;
	DatabaseReference reference;

	void Start()
	{
		userId = SystemInfo.deviceUniqueIdentifier;
		reference = FirebaseDatabase.DefaultInstance.RootReference;

	}
	private void Update()
	{
		ReadDatabase();
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
						 Debug.Log("HR: " + HR);
					 }
				 }
			 }
		 });

		reference.Child("POLOHPPG")
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
				string s = "";
				int arr_index = 0;
				double[] value_arr = new double[4];
				foreach (DataSnapshot childSnapshot in snapshot.Children)
				{
					if (double.TryParse(childSnapshot.Value.ToString(), out double value))
					{
						value_arr[arr_index] = value;
						arr_index++;
						s += $"{value};";
					}
				}
				Debug.Log("(" + s + ")");
			}
		}
	});


	}
}