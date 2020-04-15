using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoginMenuAuthenticationControler : MonoBehaviour
{

	public InputField emailInput, passwordInput; 
	public GameSceneManager gameSceneManager;
	bool loginSuccessful;
	bool registerSuccessful;
	public Text FirebaseErrorText;
	
	string loginError = "";
	string registerError = "";

	
    protected virtual void Start()
    {		
		loginSuccessful = false;
		registerSuccessful = false;
		FirebaseErrorText.enabled = false;
    }	
	
	public async void Login()
	{
		print("BEFORE!");
		await LoginToDB_ReturnTask();
		print("AFTER!");
		
		if (loginSuccessful == true)
		{
			goToMainMenu();
		}
		else{
			FirebaseErrorText.text = loginError;
			StartCoroutine(ShowMessage(3));			
		}
	}
	
	public Task LoginToDB_ReturnTask() // Note the return type, not async
	{
		
		return FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				

				AuthError msg = (AuthError)e.ErrorCode;
				loginError = msg.ToString();
				print("Error: " + loginError);
				
				return;
			}
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				
				AuthError msg = (AuthError)e.ErrorCode;
				loginError = msg.ToString();
				print("Error: " + loginError);				
				
				return;
			}
			if (task.IsCompleted) {
				print("Login successful!");
				loginSuccessful = true;				
			}
		return;
		});		
	}




	public async void Register()
	{
		print("BEFORE!");
		await RegisterToDB_ReturnTask();
		print("AFTER!");
		
		if (registerSuccessful == true)
		{
			goToMainMenu();
		}
		else{
			FirebaseErrorText.text = registerError;
			StartCoroutine(ShowMessage(3));			
		}
	}
	
	public Task RegisterToDB_ReturnTask() // Note the return type, not async
	{
		
		return FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(task => {
			
			if (task.IsCanceled) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				

				AuthError msg = (AuthError)e.ErrorCode;
				registerError = msg.ToString();
				print("Error: " + registerError);
				
				return;
			}
			if (task.IsFaulted) {
				Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;				
				
				AuthError msg = (AuthError)e.ErrorCode;
				registerError = msg.ToString();
				print("Error: " + registerError);				
				
				return;
			}
			if (task.IsCompleted) {
				print("Registration complete successfully!");
				registerSuccessful = true;				
			}
		return;
		});		
	}




	public IEnumerator ShowMessage (float delay)
	{
		FirebaseErrorText.enabled = true;
		yield return new WaitForSeconds(delay);
		FirebaseErrorText.enabled = false;
	}
	
	
	void goToMainMenu(){
		gameSceneManager.GoToMainMenu(); //Go to Main menu.
	}

}
