﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public void PlayGame()
	{
		//SceneManager.LoadScene(0);
		SceneManager.LoadScene("ActivitiesMenu");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void Logout()
	{
		//Application.Quit();
		SceneManager.LoadScene("LoginMenu");
	}
	
}
