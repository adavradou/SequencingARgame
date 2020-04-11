using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LoginMenuAuthenticationControler : MonoBehaviour
{

	public InputField emailInput, passwordInput; 
	public GameSceneManager gameSceneManager;
	bool loginSuccessful;

	
    protected virtual void Start()
    {		
		loginSuccessful = false;
    }	
	
	public void Login()
	{
		//bool loginSuccessful = true;
		
		FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				GetErrorMessage((AuthError)e.ErrorCode);
				return;
			}
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				GetErrorMessage((AuthError)e.ErrorCode);
				return;
			}
			if (task.IsCompleted) {
				print("Login successful!");
				loginSuccessful = true;				
			}

		});
		
		if (loginSuccessful == true)
		{
			goToMainMenu();
		}	
		else{
			print("loginSuccessful was not true");
		}		
	}


	public void Register()
	{
		
		if (emailInput.text.Equals("") && passwordInput.text.Equals(""))
		{
			print("Please enter an email and password to register");
			return;
		}
		FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				GetErrorMessage((AuthError)e.ErrorCode);
				return;
			}
			
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				GetErrorMessage((AuthError)e.ErrorCode);
				return;
			}
			
			if (task.IsCompleted) {
				print("Registration complete successfully!");
			}

		});		
	}

	
	void GetErrorMessage(AuthError errorCode)
	{
		string msg = "";
		msg = errorCode.ToString();
			
		print(msg);
	}
	
	
	void goToMainMenu(){
		print("I am in.");
		gameSceneManager.GoToSaveLoadMenu(); //Go to Main menu.
		//FindObjectOfType<GameSceneManager>().GoToMainMenu(); //Gi to Main menu.

		print("... and out.");
	}

}
