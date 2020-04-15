using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class FirebaseManager : MonoBehaviour
{
	
	public Text StatisticsBoardText;

	private GameStatistics data;	
	private string DATA_URL = "https://sequencingargame.firebaseio.com/";	
	private DatabaseReference databaseReference;
	
	string usrEmail = "adavradougmailcom";
	string levelName = "level_1_showering";
	
	bool loadSuccessful;
	string loadError = "";
	
	List<string> AttemptsList = new List<string>();   
	List<string> DurationList = new List<string>();   
	List<string> LevelPassedList = new List<string>();   


	void Start()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATA_URL);		
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;				
		StatisticsBoardText.enabled = false;
	}


	public async void LoadData()
	{	
		/*The data of each level for the selected activity are loaded separately and all the stats are saved on a string. 
		Then they are displayed on a GUI text.*/
		
		string activityStatisticsText = ""; //Keeps the stats of all 3 (?) levels to display them together.
		
		resetLevelValues();			
		await LoadData_ReturnTask("level_1_showering"); //Load the data for the 1st level.		
		
		if (loadSuccessful == true)
		{					
			activityStatisticsText = "Level 1: \n";			
			activityStatisticsText = activityStatisticsText + getLevelStatistics(AttemptsList, DurationList, LevelPassedList) + "\n";			
		}
		else{
			//TO FIX: DISPLAY ERROR MESSAGE.
		}
		
		resetLevelValues();		
		
		await LoadData_ReturnTask("level_2_showering"); //Load the data for the 2nd level.		
		
		if (loadSuccessful == true)
		{					
			activityStatisticsText = activityStatisticsText + "Level 2: \n";			
			activityStatisticsText = activityStatisticsText + getLevelStatistics(AttemptsList, DurationList, LevelPassedList) + "\n";			
		}
		else{
			//TO FIX: DISPLAY ERROR MESSAGE.
		}
			
		//Display the statistics of all levels of this activity on the UI.
		StatisticsBoardText.text = activityStatisticsText;
		StatisticsBoardText.enabled = true;		
		
	}	


	public Task LoadData_ReturnTask(string level)
	{
		return databaseReference.Child(usrEmail  + "_" + level).GetValueAsync().ContinueWith((task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				

				AuthError msg = (AuthError)e.ErrorCode;
				loadError = msg.ToString();
				print("Error: " + loadError);
				
				return;
			}
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				
				AuthError msg = (AuthError)e.ErrorCode;
				loadError = msg.ToString();
				print("Error: " + loadError);				
				
				return;
			}
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				
				string playerData = snapshot.GetRawJsonValue();
								
				foreach(var child in snapshot.Children)
				{
					string t = child.GetRawJsonValue();
					
					GameStatistics extractedData = JsonUtility.FromJson<GameStatistics>(t);
										

					AttemptsList.Add(extractedData.gameAttempts);
					DurationList.Add(extractedData.gameDuration);
					LevelPassedList.Add(extractedData.levelPassed);
				}
				
				
				print("Data loaded successfully!");
				loadSuccessful = true;		
			}

		}));	
	}

	
	public void SaveData(string attempts, string time, string passed)
	{
		
		//DateTime now = DateTime.Now;
	
		data = new GameStatistics(attempts, time, passed);
		
		string jsonData = JsonUtility.ToJson(data);		

		databaseReference.Child(usrEmail  + "_" + levelName).Child("id_" + UnityEngine.Random.Range(0, 100000)).SetRawJsonValueAsync(jsonData);  


		print("Data saved.");
	}
	
	private void resetLevelValues()
	{
		loadSuccessful = false;
		//Clear the lists (before loading data of each level).
		AttemptsList.Clear(); 
		DurationList.Clear();
		LevelPassedList.Clear(); 		
	}
	
	private string getLevelStatistics(List<string> attempts, List<string> time, List<string> passed)
	{
		//Computes the statistics of the user's loaded data.
		string levelStatsText;		
		int timesPlayed = passed.Count;		
		string best_time = findMinFloat(time);		
		string fewer_attempts = findMinInt(attempts);		
		int times_passed = countOccurences(passed, "yes");		
		double success_rate =  Math.Round(((double)(times_passed / (double)timesPlayed)) * 100, 2);		
		
		levelStatsText = ("\t Time played: " + timesPlayed.ToString() + "\n" + "\t Fewer attempts: " + fewer_attempts + "\n" + "\t Best time: " + best_time + "\n" + "\t Succcess rate: " + success_rate.ToString() + "%\n");
				
		return levelStatsText;
	}
	
	private int countOccurences(List<string> list, string word)  
	{ 

		int count = 0; 
		for (int i = 0; i < list.Count; i++)  
		{ 
			  
		if (word.Equals(list[i])) 
			count++; 
		} 
	  
		return count; 
	} 
  
	
	
	private string findMinInt (List<string> stringList)
	{
		
		int lowest_price = 1000;
		List<int> intList = stringList.ConvertAll(int.Parse);
		
		foreach(int a in intList){
			if(a <= lowest_price){
				lowest_price = a;
				Console.WriteLine(a);
			}
		}	
		return lowest_price.ToString();
		
	}
	
	private string findMinFloat (List<string> stringList)
	{
		
		float lowest_price = 1000;
		List<float> intList = stringList.ConvertAll(float.Parse);
		
		foreach(float a in intList){
			if(a <= lowest_price){
				lowest_price = a;
				Console.WriteLine(a);
			}
		}
		return lowest_price.ToString();
		
	}
	
	/*
	private string ListToText(List<string> list)
	{
		string result = "";
		foreach(var listMember in list)
		{
			result += listMember.ToString() + "\n";
		}
		return result;
	}
	*/
	
}
