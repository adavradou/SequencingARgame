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
using System.Linq;


public class LevelVisualizationManager : MonoBehaviour
{
	
	string usrEmail = CurrentUser.getUserEmail(); //Get the current user's email. 

	private string DATA_URL = "https://sequencingargame.firebaseio.com/";	
	private DatabaseReference databaseReference;
	
	string loadError = "";
	List<string> LevelPassedList = new List<string>();   

	bool levelPassedFlag;
	
	public Button showerButton_level_2;
	public Button showerButton_level_3;
	
	public Button schoolButton_level_2;
	public Button schoolButton_level_3;	
	
	public Button teethButton_level_2;
	public Button teethButton_level_3;	


	void Start()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATA_URL);		
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;		
		
		//Remove the "@" and "." from the email, as it will be used for  username on the Firebase.
		usrEmail = new string ((from c in usrEmail
						  where char.IsLetterOrDigit(c)
						  select c
			   ).ToArray());
		
		VisualizePassedLevels();				

	}
	
	void resetValues()
	{
		levelPassedFlag = false;
		//Clear the list (before loading data of each level).
		LevelPassedList.Clear(); 			
	}

	public async void VisualizePassedLevels()
	{
		//For the buttons of the Showering activity. 
		resetValues();
		await LoadDataManager("Showering", "level_1");
		
		if (levelPassedFlag == false)
		{
			showerButton_level_2.gameObject.SetActive(false);
			showerButton_level_3.gameObject.SetActive(false);
		}
		else{
			resetValues();
			await LoadDataManager("Showering", "level_2");	
			
			if (levelPassedFlag == false)
			{
				showerButton_level_3.gameObject.SetActive(false);
			}				
		}		
		
		
		//For the buttons of the School activity. 
		resetValues();
		await LoadDataManager("School", "level_1");
		
		if (levelPassedFlag == false)
		{
			schoolButton_level_2.gameObject.SetActive(false);
			schoolButton_level_3.gameObject.SetActive(false);
		}
		else{
			resetValues();
			await LoadDataManager("School", "level_2");	
			
			if (levelPassedFlag == false)
			{
				schoolButton_level_3.gameObject.SetActive(false);
			}
		}			
	

		//For the buttons of the Teeth activity. 
		resetValues();
		await LoadDataManager("Teeth", "level_1");
		
		if (levelPassedFlag == false)
		{
			teethButton_level_2.gameObject.SetActive(false);
			teethButton_level_3.gameObject.SetActive(false);
		}
		else{
			resetValues();
			await LoadDataManager("Teeth", "level_2");	
			
			if (levelPassedFlag == false)
			{
				teethButton_level_3.gameObject.SetActive(false);
			}
		}		
		
	}
	
	public Task LoadDataManager(string activityName, string activityLevel)
	{
		return databaseReference.Child(usrEmail + "_"+ activityName + "_" + activityLevel).GetValueAsync().ContinueWith((task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				

				AuthError msg = (AuthError)e.ErrorCode;
				loadError = msg.ToString();
				print("Error: " + loadError);				
			}
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				
				AuthError msg = (AuthError)e.ErrorCode;
				loadError = msg.ToString();
				print("Error: " + loadError);			
				
			}
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				
				string playerData = snapshot.GetRawJsonValue();
								
				foreach(var child in snapshot.Children)
				{
					string t = child.GetRawJsonValue();
					
					GameStatistics extractedData = JsonUtility.FromJson<GameStatistics>(t);										

					LevelPassedList.Add(extractedData.levelPassed);	
				}	
				
				//If the level is not passed, hide next levels (buttons).
				int times_passed = countOccurences(LevelPassedList, "yes");	
				if (times_passed == 0 )
					levelPassedFlag = false;
				else
					levelPassedFlag = true;				
			}	

		}));			
		
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

}
