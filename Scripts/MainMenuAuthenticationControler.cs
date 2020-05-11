using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class MainMenuAuthenticationControler : MonoBehaviour
{
	
	public void Logout()
	{
		if (FirebaseAuth.DefaultInstance.CurrentUser != null)
		{
			FirebaseAuth.DefaultInstance.SignOut();
			print("User logged out.");
		}
	}
	
	
	void GetErrorMessage(AuthError errorCode)
	{
		string msg = "";
		msg = errorCode.ToString();
			
		print(msg);
	}
}
