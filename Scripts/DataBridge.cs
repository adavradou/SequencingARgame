using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class DataBridge : MonoBehaviour
{

	public InputField usernameInput, passwordInput;
	
	private Player data;
	
	private string DATA_URL = "https://sequencingargame.firebaseio.com/";
	
	private DatabaseReference databaseReference;
	
	string usrEmail = "adavradou@gmail.com";


	void Start()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATA_URL);
		
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
				
		
	}
	
	public void SaveData()
	{
		if (usernameInput.text.Equals("") && passwordInput.text.Equals(""))
		{
			print("No data found.");
			return;
		}
		
		data = new Player(usernameInput.text, passwordInput.text);
		
		string jsonData = JsonUtility.ToJson(data);
		
			
		//The random.range creats a uniqui ID, so that users are not overwrited.
		//databaseReference.Child("Users" + Random.Range(0, 100000)).SetRawJsonValueAsync(jsonData);
		//databaseReference.Child("adavradougmailcom").SetRawJsonValueAsync(jsonData);

		//databaseReference.Child(usrEmail).SetRawJsonValueAsync(jsonData);
		databaseReference.Child("adavradougmailcom").Child("level1showering").SetRawJsonValueAsync(jsonData);


		//databaseReference.Child("Users").SetRawJsonValueAsync(jsonData);		
		print("Data saved.");
	}


	public void LoadData()
	{
		FirebaseDatabase.DefaultInstance.GetReferenceFromUrl(DATA_URL).GetValueAsync().ContinueWith((task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				//GetErrorMessage((AuthError)e.ErrorCode);
				return;
			}
			
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				//GetErrorMessage((AuthError)e.ErrorCode);
				return;
			}
			
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				
				string playerData = snapshot.GetRawJsonValue();
								
				foreach(var child in snapshot.Children)
				{
					string t = child.GetRawJsonValue();
					
					Player extractedData = JsonUtility.FromJson<Player>(t);
										
					print("The username is: " + extractedData.attempts);
					print("The password is: " + extractedData.passed);
				}
				
				
				print("Data loaded successfully!");
			}

		}));	
	}

/*
	void GetErrorMessage(AuthError errorCode)
	{
		string msg = "";
		msg = errorCode.ToString();
			
		print(msg);
	}
	*/

}
