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
	public Text FirebaseSuccessText;
	
	string loginError = "";
	string registerError = "";

	
    protected virtual void Start()
    {		
		loginSuccessful = false;
		registerSuccessful = false;
		FirebaseErrorText.enabled = false;
		FirebaseSuccessText.enabled = false;
    }	
	
	public async void Login()
	{
		await LoginToDB_ReturnTask();
		
		if (loginSuccessful == true)
		{
			//GetComponent<CurrentUser>().setUserEmail(emailInput.text);
			//GetComponent<CurrentUser>().setUserEmail(emailInput.text);
			CurrentUser.setUserEmail(emailInput.text);
			goToMainMenu();
		}
		else{
			FirebaseErrorText.text = loginError;
			StartCoroutine(ShowMessage(3, FirebaseErrorText));			
		}
	}
	
	private Task LoginToDB_ReturnTask() // Note the return type, not async
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
		await RegisterToDB_ReturnTask();
		
		if (registerSuccessful == true)
		{
			print("Registration successful!");
			FirebaseSuccessText.text = "Registration successful!";
			StartCoroutine(ShowMessage(3, FirebaseSuccessText));		
		}
		else{
			FirebaseErrorText.text = registerError;
			StartCoroutine(ShowMessage(3, FirebaseErrorText));			
		}
	}
	
	private Task RegisterToDB_ReturnTask() // Note the return type, not async
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




	IEnumerator ShowMessage (float delay, Text buttonName)
	{
		buttonName.enabled = true;
		yield return new WaitForSeconds(delay);
		buttonName.enabled = false;
	}
	
	
	void goToMainMenu(){
		gameSceneManager.GoToMainMenu(); //Go to Main menu.
	}

}
