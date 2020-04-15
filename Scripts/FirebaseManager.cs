using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class FirebaseManager : MonoBehaviour
{

	private GameStatistics data;
	
	private string DATA_URL = "https://sequencingargame.firebaseio.com/";
	
	private DatabaseReference databaseReference;
	
	string usrEmail = "adavradougmailcom";
	string levelName = "level_1_showering";


	void Start()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATA_URL);
		
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
				
		
	}
	
	public void SaveData(string attempts, string time, string passed)
	{
		
		DateTime now = DateTime.Now;
	
		data = new GameStatistics(attempts, time, passed);
		
		string jsonData = JsonUtility.ToJson(data);
		

		databaseReference.Child(usrEmail  + "_" + levelName).Child("id_" + UnityEngine.Random.Range(0, 100000)).SetRawJsonValueAsync(jsonData);  


		print("Data saved.");
	}

}
